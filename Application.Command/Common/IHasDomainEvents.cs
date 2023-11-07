namespace Application.Command.Common;

public interface IHasDomainEvents
{
    void AddDomainEvent(DomainEvent domainEvent);

    void ClearDomainEvents();

    public IReadOnlyCollection<DomainEvent> DomainEvents { get; }
}
