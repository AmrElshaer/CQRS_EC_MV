using Application.Entities;
using Application.Infrastructure.Persistence;

namespace Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler:IRequestHandler<CreateOrderCommand,Guid>
{
    private readonly ApplicationDbContext _db;

    public CreateOrderCommandHandler(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var count = _db.Orders.Count();
        var orderItems = request.OrderItems.Select(x=>new OrderItem(x.ProductId,x.Quantity)).ToList();

        var order= Order.Create(
             count
            ,request.CustomerId, 
            orderItems
            );
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return order.Id;
    }
}
