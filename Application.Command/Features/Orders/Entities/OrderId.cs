using Application.Command.Common;

namespace Application.Command.Features.Orders.Entities;

public class OrderId : EntityId
{
    private OrderId(Guid id) : base(id) { }

    public static OrderId From(Guid id)
    {
        return new(id);
    }
}
