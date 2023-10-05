#nullable disable
using Application.Shared.QueryModels.Orders;

namespace Application.Shared.Events.IntegrationEvents.Orders;

public class OrderCreatedIntegrationEvent : IntegrationEvent<OrderQueryModel>
{
}
