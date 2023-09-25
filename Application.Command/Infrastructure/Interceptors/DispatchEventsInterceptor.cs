#nullable disable
using System.Data;
using Application.Command.Common;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Command.Infrastructure.Interceptors;

public class DispatchEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;
    private readonly ICapPublisher _integrationEventPublisher;

    public DispatchEventsInterceptor(IMediator mediator, ICapPublisher integrationEventPublisher)
    {
        _mediator = mediator;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        throw new NotSupportedException("SavingChanges is not supported. Use SavingChangesAsync instead");
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync
        (DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var database = eventData.Context?.Database;
        ArgumentNullException.ThrowIfNull(database);

        await using var transaction = database.CurrentTransaction ?? await database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

        try
        {
            await DispatchDomainEvents(eventData.Context, cancellationToken);
            var res = await base.SavingChangesAsync(eventData, result, cancellationToken);
            await DispatchIntegrationEvents(eventData.Context, transaction, cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return res;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }
    }

    private async Task DispatchIntegrationEvents(DbContext context, IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        if (context == null)
            return;

        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.IntegrationEvents.Any())
            .Select(e => e.Entity);

        var integrationEvents = entities
            .SelectMany(e => e.IntegrationEvents)
            .ToList();

        if (integrationEvents.Count == 0)
        {
            return;
        }

        entities.ToList().ForEach(e => e.ClearIntegrationEvents());

        _integrationEventPublisher.Transaction.Value = ActivatorUtilities.CreateInstance<SqlServerCapTransaction>(_integrationEventPublisher.ServiceProvider).Begin(transaction);

        foreach (var integrationEvent in integrationEvents)
        {
            await _integrationEventPublisher.PublishAsync(integrationEvent.GetType().Name, integrationEvent, cancellationToken: cancellationToken);
        }
    }

    private async Task DispatchDomainEvents(DbContext context, CancellationToken cancellationToken = default)
    {
        if (context == null)
            return;

        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);
    }
}
