using System.ComponentModel.DataAnnotations.Schema;
using Application.Shared.Events.IntegrationEvents;

namespace Application.Command.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

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

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void AddIntegrationEvent(IntegrationEvent integrationEvent)
    {
        _integrationEvents.Add(integrationEvent);
    }

    public void RemoveIntegrationEvent(IntegrationEvent integrationEvent)
    {
        _integrationEvents.Remove(integrationEvent);
    }

    public void ClearIntegrationEvents()
    {
        _integrationEvents.Clear();
    }
}
