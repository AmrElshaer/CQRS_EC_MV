using Application.Shared.Events.IntegrationEvents;

namespace Application.Command.Common;

public interface IHasIntegrationEvents
{
    void AddIntegrationEvent(IntegrationEvent integrationEvent);

    void ClearIntegrationEvents();

    public IReadOnlyCollection<IntegrationEvent> IntegrationEvents { get; }
}
