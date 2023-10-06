namespace Application.Shared.QueryModels.Orders;

public class OrderQueryModel
{
    public Guid Id { get; init; }

    public string CustomerName { get; init; } = default!;

    public string OrderNumber { get; init; } = default!;

    public double Latitude { get; init; }

    public double Longitude { get; init; }

    public IList<OrderItemViewQueryModel> OrderItemViewQueryModels { get; init; }= new List<OrderItemViewQueryModel>();
}

public class OrderItemViewQueryModel
{
    public string ProductName { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public int Quantity { get; init; }
}
