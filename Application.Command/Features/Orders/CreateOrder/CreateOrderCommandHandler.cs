
using Application.Command.Common;
using Application.Command.Entities;
using Application.Command.Infrastructure.Persistence;
using Application.Command.ValueObjects;
using MassTransit;

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
            return Result.Fail<Guid>(locationResult.Error);
        }

        var orderItems = request.OrderItems
            .Select(x => (x.ProductId, x.Quantity))
            .ToList();
        var order = Order.Create(count, request.CustomerId, locationResult.Value, orderItems);

        if (order.Failure)
        {
            return Result.Fail<Guid>(order.Error);
        }
        _db.Orders.Add(order.Value);
        await _db.SaveChangesAsync(cancellationToken);

        return Result.Ok(order.Value.Id);


    }

}
