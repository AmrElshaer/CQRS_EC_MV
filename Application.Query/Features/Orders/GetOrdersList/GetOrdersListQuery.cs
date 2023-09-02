using Application.Query.Common.Extensions;
using Application.Query.Infrastructure.Persistance;
using Application.Shared.QueryModels.Orders;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Application.Query.Features.Orders.GetOrdersList;

public record GetOrdersListQuery(string Number, int Offset, int Limit) : IRequest<List<OrderQueryModel>>;

public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderQueryModel>>
{
    private readonly ReadDbContext _context;

    public GetOrdersListQueryHandler(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderQueryModel>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.OrderMaterializedView.AsQueryable()
            .WhereIf(!string.IsNullOrEmpty(request.Number), x => x.OrderNumber.Contains(request.Number));

        var res = await query
            .Skip(request.Offset)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);

        return res;
    }
}
