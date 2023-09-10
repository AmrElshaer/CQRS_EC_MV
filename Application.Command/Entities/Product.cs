using Application.Command.Common;
using Application.Command.ValueObjects;

namespace Application.Command.Entities;

public class Product : BaseEntity
{
    public string Name { get; init; } = default!;

    public Money Price { get; init; } = default!;

    private Product() { }

    public Product(string name, Money price)
    {
        Name = Argument.IsNotNullOrWhiteSpace(name);
        Price = Argument.IsNotNull(price);
    }
}
