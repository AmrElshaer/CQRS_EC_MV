namespace Application.Command.Features.Orders.CreateOrder;

public record CreateOrderCommand(Guid CustomerId, double Latitude, double Longitude, List<CreateOrderItemDto> OrderItems) : IRequest<Guid>;

public record CreateOrderItemDto(Guid ProductId, int Quantity);
