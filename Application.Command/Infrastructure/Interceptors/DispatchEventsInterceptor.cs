#nullable disable
using Application.Command.Common;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
        await DispatchDomainEvents(eventData.Context, cancellationToken);
        await DispatchIntegrationEvents(eventData.Context, cancellationToken);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchIntegrationEvents(DbContext context, CancellationToken cancellationToken = default)
    {
        if (context == null)
            return;

        var entities = context.ChangeTracker
            .Entries<IHasIntegrationEvents>()
            .Where(e => e.Entity.IntegrationEvents.Any())
            .Select(e => e.Entity);

        var integrationEvents = entities
            .SelectMany(e => e.IntegrationEvents)
            .ToList();

        if (integrationEvents.Count == 0)
        {
            return;
        }

        foreach (var integrationEvent in integrationEvents)
        {
            await _integrationEventPublisher.PublishAsync(integrationEvent.GetType().Name, integrationEvent, cancellationToken: cancellationToken);
        }

        foreach (var entity in entities)
        {
            entity.ClearIntegrationEvents();
        }
    }

    private async Task DispatchDomainEvents(DbContext context, CancellationToken cancellationToken = default)
    {
        if (context == null)
            return;

        var entities = context.ChangeTracker
            .Entries<IHasDomainEvents>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        foreach (var e in entities)
        {
            e.ClearDomainEvents();
        }
    }
}
