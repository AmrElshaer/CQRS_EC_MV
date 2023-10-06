using Application.Command.Common;
using Application.Command.Common.Enumerations;
using Application.Command.ValueObjects;
using Application.Shared.Events.IntegrationEvents.Orders;
using MassTransit;

namespace Application.Command.Entities;

public class Order : BaseEntity
{
    public string Number { get; private set; } = default!;

    public OrderStatus Status { get; private set; }

    public Location Location { get; private set; } = default!;

    public Guid CustomerId { get; private set; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private readonly List<OrderItem> _orderItems = new();

    private Order() { }

    private Order(string number,
        Guid customerId,
        Location location
    )
    {

        Number = number;
        CustomerId = customerId;
        Status = OrderStatus.Created;
        Location = location;
        AddIntegrationEvent(new OrderCreatedIntegrationEvent()
        {
            OrderId = Id
        });
    }

    public static Result<Order> Create(
        int count,
        Guid customerId,
        Location location,
        IReadOnlyCollection<(Guid ProductId,int Quantity)> orderItems)
    {
        
        var number = $"O-{count + 1}";
        var order=new Order(number, customerId, location);
        var orderItemsResults = orderItems
            .Select(x => OrderItem.Create(x.ProductId,order.Id, x.Quantity))
            .ToList();

        var orderItemRes = orderItemsResults.FirstOrDefault(oi => oi.Failure);

        if (orderItemRes != null)
        {
            return Result.Fail<Order>(orderItemRes.Error);
        }
        order._orderItems.AddRange(orderItemsResults.Select(oi=>oi.Value));
        return Result.Ok(order);
    }
}
