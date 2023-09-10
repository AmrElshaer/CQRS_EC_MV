using Application.Command.Common;

namespace Application.Command.Entities;

public class Customer : BaseEntity
{
    public string Name { get; init; } = default!;

    private Customer() { }

    public Customer(string name)
    {
        Name = Argument.IsNotNullOrWhiteSpace(name);
    }
}
