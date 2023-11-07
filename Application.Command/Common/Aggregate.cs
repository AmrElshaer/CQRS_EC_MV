using System.ComponentModel.DataAnnotations.Schema;
using Application.Shared.Events.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;

namespace Application.Command.Common;

public abstract class Aggregate<TId>:Entity<TId>,IHasIntegrationEvents ,IHasDomainEvents where TId : EntityId
{
    protected Aggregate(TId id) : base(id)
    {
        
    }

    protected Aggregate()
    {
        
    }
    private readonly List<DomainEvent> _domainEvents = new();
    private readonly List<IntegrationEvent> _integrationEvents = new();

    [NotMapped]
    public IReadOnlyCollection<IntegrationEvent> IntegrationEvents => _integrationEvents.AsReadOnly();

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

  

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void AddIntegrationEvent(IntegrationEvent integrationEvent)
    {
        _integrationEvents.Add(integrationEvent);
    }

   

    public void ClearIntegrationEvents()
    {
        _integrationEvents.Clear();
    }
}
