using Application.Command.Entities;
using Application.Command.Infrastructure.Persistence;
using Application.Command.ValueObjects;

namespace Application.Command.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly WriteDbContext _db;

    public CreateOrderCommandHandler(WriteDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var count = _db.Orders.Count();
        var orderItems = request.OrderItems.Select(x => new OrderItem(x.ProductId, x.Quantity)).ToList();
        var address = Address.Create(request.Latitude, request.Longitude);

        var order = Order.Create(
            count
            , request.CustomerId,
            address,
            orderItems
        );

        _db.Orders.Add(order);
        await _db.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
