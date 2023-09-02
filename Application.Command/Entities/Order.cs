using Application.Command.Common;
using Application.Command.Common.Enumerations;
using Application.Command.ValueObjects;
using Application.Shared.Events.IntegrationEvents.Orders;

namespace Application.Command.Entities;

public class Order : BaseEntity
{
    public string Number { get; private set; } = default!;

    public OrderStatus Status { get; private set; }

    public Address Address { get; private set; } = default!;

    public Guid CustomerId { get; private set; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private readonly List<OrderItem> _orderItems = new();

    private Order() { }

    public static Order Create(int count, Guid customerId, Address address, IReadOnlyCollection<OrderItem> orderItems)
    {
        var order = new Order()
        {
            Number = $"O-{count + 1}",
            CustomerId = customerId,
            Status = OrderStatus.Created,
            Address = address
        };

        order._orderItems.AddRange(Argument.IsNotEmpty(orderItems));

        order.AddIntegrationEvent(new OrderCreatedIntegrationEvent()
        {
            OrderId = order.Id
        });

        return order;
    }
}
