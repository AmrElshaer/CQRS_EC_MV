﻿using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace Application.Command.Features.Orders.CreateOrder;

public class CreateOrderEndpoint : Endpoint<CreateOrderCommand, IActionResult>
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

    public override async Task<IActionResult> ExecuteAsync(CreateOrderCommand req, CancellationToken ct)
    {
        var res = await _mediator.Send(req, ct);

        return res.Match<IActionResult>(
            success => new OkObjectResult(success),
            failure => new BadRequestObjectResult(failure)
        );
    }
}
