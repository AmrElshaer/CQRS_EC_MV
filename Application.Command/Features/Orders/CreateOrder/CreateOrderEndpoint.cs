using FastEndpoints;

namespace Application.Command.Features.Orders.CreateOrder;

public class CreateOrderEndpoint : Endpoint<CreateOrderCommand, Guid>
{
    private readonly IMediator _mediator;

    public CreateOrderEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/orders");
    }

    public override async Task<Guid> ExecuteAsync(CreateOrderCommand req, CancellationToken ct)
    {
        return await _mediator.Send(req, ct);
    }
}
