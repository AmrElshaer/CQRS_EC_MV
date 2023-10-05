using Application.Command.Common;

namespace Application.Command.Features.Orders.CreateOrder;

public record CreateOrderCommand(Guid CustomerId,
    double Latitude,
    double Longitude,
    List<CreateOrderItemDto> OrderItems) : IRequest<Result<Guid>>;

public record CreateOrderItemDto(Guid ProductId, int Quantity);
