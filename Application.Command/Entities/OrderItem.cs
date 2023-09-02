namespace Application.Command.Entities;

public class OrderItem
{
    public Guid OrderId { get; private set; }

    public Guid ProductId { get; private set; }

    public int Quantity { get; private set; }

    private OrderItem() { }

    public OrderItem(Guid productId, int quantity)
    {
        ProductId = Argument.IsNotDefault(productId);
        Quantity = Argument.IsGreaterThan(quantity, 0);
    }
}
