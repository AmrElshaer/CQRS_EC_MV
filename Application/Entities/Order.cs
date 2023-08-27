using Application.Common;

namespace Application.Entities;

public class Order:BaseEntity
{
    public int Number { get; private set; }

    public Guid CustomerId { get; set; }

    public IReadOnlyCollection<OrderItem> OrderItems=>_orderItems.AsReadOnly();
    private readonly List<OrderItem> _orderItems=new();
}
