using Application.Common;
using Application.Common.Enumerations;

namespace Application.Entities;

public class Order : BaseEntity
{
    public string Number { get; private set; } = default!;

    public OrderStatus Status { get; set; }

    public Guid CustomerId { get; set; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private readonly List<OrderItem> _orderItems = new();

    private Order() { }

    public static Order Create(int count, Guid customerId, IReadOnlyCollection<OrderItem> orderItems)
    {
        var order = new Order()
        {
            Number = $"O-{count + 1}",
            CustomerId = customerId,
            Status = OrderStatus.Created
        };

        order._orderItems.AddRange(Argument.IsNotEmpty(orderItems));

        return order;
    }
}
