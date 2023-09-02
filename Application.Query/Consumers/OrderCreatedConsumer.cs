using Application.Query.Infrastructure.Persistance;
using Application.Shared.Events.IntegrationEvents.Orders;
using MassTransit;

namespace Application.Query.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedIntegrationEvent>
{
    private readonly ReadDbContext _context;

    public OrderCreatedConsumer(ReadDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
    {
        await _context.OrderMaterializedView.InsertOneAsync(context.Message.Payload);
    }
}
