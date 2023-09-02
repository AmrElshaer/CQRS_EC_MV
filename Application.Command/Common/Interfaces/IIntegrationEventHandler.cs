using Application.Shared.Events.IntegrationEvents;

namespace Application.Command.Common.Interfaces;

public interface IIntegrationEventHandler<in TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}
