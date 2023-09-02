using Application.Command.Common;

namespace Application.Command.Entities;

public class Customer : BaseEntity
{
    public string Name { get; set; } = default!;
}
