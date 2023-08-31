namespace Application.QueryModels.Orders;

public class OrderQueryModel
{
    public Guid Id { get; init; }

    public string CustomerName { get; init; } = default!;

    public string OrderNumber { get; init; } = default!;

    public IReadOnlyList<OrderItemViewQueryModel> OrderItemViewQueryModels { get; init; } = default!;
}

public record OrderItemViewQueryModel(Guid Id, string ProductName, decimal Price, int Quantity);
