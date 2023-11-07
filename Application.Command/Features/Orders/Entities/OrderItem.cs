using Application.Command.Common;
using Application.Command.Features.Products.Entities;

namespace Application.Command.Features.Orders.Entities;

public class OrderItem
{
    public OrderId OrderId { get; private set; }=default!;

    public ProductId ProductId { get; private set; }=default!;

    public int Quantity { get; private set; }

    private OrderItem() { }

    private  OrderItem(ProductId productId,OrderId orderId, int quantity)
    {
        ProductId = productId;
        Quantity =quantity;
        OrderId = orderId;
    }

    public static Result<OrderItem>  Create(ProductId productId,OrderId orderId, int quantity)
    {
        if (quantity<=0)
        {
            return  ValidationException.LessThanOrEqualZero(nameof(quantity));
        }

        return new OrderItem(productId,orderId,quantity);
    }
    
}
