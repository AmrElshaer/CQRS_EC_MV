using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace Application.Command.Features.Orders.CreateOrder;

public class CreateOrderEndpoint : Endpoint<CreateOrderCommand>
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
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateOrderCommand req, CancellationToken ct)
    {
        var res = await _mediator.Send(req, ct);

        if (res.Failure)
        {
            ThrowError(res.Error.Message);
        }
        await SendAsync( res.Value, cancellation:ct);
    }
}
