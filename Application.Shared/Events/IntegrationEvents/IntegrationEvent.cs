namespace Application.Shared.Events.IntegrationEvents;

public abstract class IntegrationEvent<T> : IntegrationEvent
{
    public T Payload { get; private set; } = default!;

    public void AddPayload(T payload)
    {
        Payload = payload;
    }
}

public abstract class IntegrationEvent
{
    protected Guid Id { get; set; } = Guid.NewGuid();
    protected DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
