using Application.Command.Common;
using Application.Command.Features.Products.ValueObjects;

namespace Application.Command.Features.Products.Entities;

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
