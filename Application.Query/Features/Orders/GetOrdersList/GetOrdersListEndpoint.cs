using Application.Shared.QueryModels.Orders;
using FastEndpoints;

namespace Application.Query.Features.Orders.GetOrdersList;

public class GetOrdersListEndpoint : Endpoint<GetOrdersListQuery, List<OrderQueryModel>>
{
    private readonly IMediator _mediator;

    public GetOrdersListEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/orders");
    }

    public override async Task<List<OrderQueryModel>> ExecuteAsync(GetOrdersListQuery req, CancellationToken ct)
    {
        return await _mediator.Send(req, ct);
    }
}
