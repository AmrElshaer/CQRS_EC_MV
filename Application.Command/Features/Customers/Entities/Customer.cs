using Application.Command.Common;

namespace Application.Command.Features.Customers.Entities;

public class Customer : Aggregate<CustomerId>
{
    public string Name { get; init; } = default!;

    private Customer() { }

    public Customer(CustomerId customerId, string name)
        : base(customerId)
    {
        Name = Argument.IsNotNullOrWhiteSpace(name);
    }
}
