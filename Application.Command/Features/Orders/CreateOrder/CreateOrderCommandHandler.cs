using Application.Command.Common;
using Application.Command.Features.Customers.Entities;
using Application.Command.Features.Orders.Entities;
using Application.Command.Features.Orders.ValueObjects;
using Application.Command.Features.Products.Entities;
using Application.Command.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
            .Select(x => (ProductId.From(x.ProductId), x.Quantity))
            .ToList();

        var orderId = OrderId.From(Guid.NewGuid());
        var customerId = CustomerId.From(request.CustomerId);

        var orderRes = Order.Create(count, orderId, customerId, locationResult.Value, orderItems);

        if (orderRes.Failure)
        {
            return orderRes.Error;
        }

        _db.Orders.Add(orderRes.Value);
        await _db.SaveChangesAsync(cancellationToken);

        return orderRes.Value.Id.Value;
    }
}
