using Application.Command.Common.Exceptions;
using Application.Command.Common.Interfaces;
using Application.Command.Infrastructure.Persistence;
using Application.Shared.Events.IntegrationEvents.Orders;
using Application.Shared.QueryModels.Orders;
using Dapper;
using DotNetCore.CAP;
using MassTransit;

namespace Application.Command.Features.Orders.CreateOrder;

public class OrderCreatedIntegrationEventHandler : ICapSubscribe, IIntegrationEventHandler<OrderCreatedIntegrationEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly DapperDbContext _db;

    public OrderCreatedIntegrationEventHandler(IPublishEndpoint publishEndpoint, DapperDbContext db)
    {
        _publishEndpoint = publishEndpoint;
        _db = db;
    }

    [CapSubscribe(nameof(OrderCreatedIntegrationEvent))]
    public async Task Handle(OrderCreatedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        const string sql = @$"
        SELECT 
            o.Id AS {nameof(OrderQueryModel.Id)},   
            o.Latitude AS {nameof(OrderQueryModel.Latitude)},
            o.Longitude AS {nameof(OrderQueryModel.Longitude)},
            c.Name AS {nameof(OrderQueryModel.CustomerName)},
            p.Name AS {nameof(OrderItemViewQueryModel.ProductName)},
            oi.Quantity AS {nameof(OrderItemViewQueryModel.Quantity)},
            oi.Price AS {nameof(OrderItemViewQueryModel.Price)},
        FROM Orders o
        INNER JOIN Customers c ON o.CustomerId = c.Id
        LEFT JOIN OrderItems oi ON o.Id = oi.OrderId
        INNER JOIN Products p ON oi.ProductId = p.Id
        WHERE o.Id = @OrderId
    ";

        using var dbConnection = _db.CreateConnection();

        Dictionary<Guid, OrderQueryModel> ordersDictionary = new();

        var orders = await dbConnection.QueryAsync<OrderQueryModel, OrderItemViewQueryModel, OrderQueryModel>(
            sql,
            (order, orderItem) =>
            {
                if (ordersDictionary.TryGetValue(order.Id, out var existOrder))
                {
                    order = existOrder;
                }
                else
                {
                    ordersDictionary.Add(order.Id, order);
                }

                order.OrderItemViewQueryModels.Add(orderItem);

                return order;
            },
            new
            {
                OrderId = @event.OrderId
            },
            splitOn: $"{nameof(OrderItemViewQueryModel.ProductName)}"
        );

        if (!ordersDictionary.TryGetValue(@event.OrderId, out var orderQueryModel))
        {
            throw new NotFoundException($"Order with id {@event.OrderId} not found");
        }

        @event.AddPayload(orderQueryModel);

        await _publishEndpoint.Publish(orderQueryModel, cancellationToken);
    }
}
