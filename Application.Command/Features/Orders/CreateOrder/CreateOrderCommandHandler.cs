using Application.Command.Common;
using Application.Command.Entities;
using Application.Command.Infrastructure.Persistence;
using Application.Command.ValueObjects;

namespace Application.Command.Features.Orders.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly WriteDbContext _db;

    public CreateOrderCommandHandler(WriteDbContext db)
    {
        _db = db;
    }

    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var count = _db.Orders.Count();

        var locationResult = Location.Create(request.Latitude, request.Longitude);

        if (locationResult.Failure)
        {
            return locationResult.Error;
        }

        var orderItems = request.OrderItems
            .Select(x => (x.ProductId, x.Quantity))
            .ToList();

        var orderRes = Order.Create(count, request.CustomerId, locationResult.Value, orderItems);

        if (orderRes.Failure)
        {
            return orderRes.Error;
        }

        _db.Orders.Add(orderRes.Value);
        await _db.SaveChangesAsync(cancellationToken);

        return orderRes.Value.Id;
    }
}
