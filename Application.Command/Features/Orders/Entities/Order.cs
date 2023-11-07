using Application.Command.Common;
using Application.Command.Features.Customers.Entities;
using Application.Command.Features.Orders.Enumerations;
using Application.Command.Features.Orders.ValueObjects;
using Application.Shared.Events.IntegrationEvents.Orders;

namespace Application.Command.Features.Orders.Entities;

public class Order : Aggregate<OrderId>
{
    public string Number { get; private set; } = default!;

    public OrderStatus Status { get; private set; }

    public Location Location { get; private set; } = default!;

    public CustomerId CustomerId { get; private set; }=default!;

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private readonly List<OrderItem> _orderItems = new();

    private Order() { }

    private Order
    (
        OrderId orderId,
        string number,
        CustomerId customerId,
        Location location
    )
    :base(orderId)
    {
        Number = number;
        CustomerId = customerId;
        Status = OrderStatus.Created;
        Location = location;

        AddIntegrationEvent(new OrderCreatedIntegrationEvent()
        {
            OrderId = Id.Value
        });
    }

    public static Result<Order> Create
    (
        int count,
        OrderId orderId,
        CustomerId customerId,
        Location location,
        IReadOnlyCollection<(Products.Entities.ProductId ProductId, int Quantity)> orderItems
    )
    {
        var number = $"O-{count + 1}";
        var order = new Order(orderId,number, customerId, location);

        var orderItemsResults = orderItems
            .Select(x => OrderItem.Create(x.ProductId, order.Id, x.Quantity))
            .ToList();

        var orderItemRes = orderItemsResults.FirstOrDefault(oi => oi.Failure);

        if (orderItemRes != null)
        {
            return orderItemRes.Error;
        }

        order._orderItems.AddRange(orderItemsResults.Select(oi => oi.Value));

        return order;
    }
}
