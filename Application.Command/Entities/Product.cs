using Application.Command.Common;
using Application.Command.ValueObjects;

namespace Application.Command.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = default!;

    public Money Price { get; set; } = default!;
}
