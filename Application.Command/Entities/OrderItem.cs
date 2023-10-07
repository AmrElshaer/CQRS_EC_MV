using Application.Command.Common;

namespace Application.Command.Entities;

public class OrderItem
{
    public Guid OrderId { get; private set; }

    public Guid ProductId { get; private set; }

    public int Quantity { get; private set; }

    private OrderItem() { }

    private  OrderItem(Guid productId,Guid orderId, int quantity)
    {
        ProductId = productId;
        Quantity =quantity;
        OrderId = orderId;
    }

    public static Result<OrderItem>  Create(Guid productId,Guid orderId, int quantity)
    {
        if (quantity<=0)
        {
            return  ValidationException.LessThanOrEqualZero(nameof(quantity));
        }

        return new OrderItem(productId,orderId,quantity);
    }
    
}
