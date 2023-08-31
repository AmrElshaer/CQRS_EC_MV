namespace Application.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommand(Guid CustomerId,List<CreateOrderItemDto> OrderItems) : IRequest<Guid>;
public record CreateOrderItemDto(Guid ProductId, int Quantity);
