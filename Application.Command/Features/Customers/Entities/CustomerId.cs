using Application.Command.Common;

namespace Application.Command.Features.Customers.Entities;

public class CustomerId : EntityId
{
    private CustomerId(Guid value) : base(value) { }

    public static CustomerId From(Guid id)
    {
        return new(id);
    }
}
