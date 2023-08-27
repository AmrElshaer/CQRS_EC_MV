using Application.Common;

namespace Application.Entities;

public class Customer:BaseEntity
{
    public string Name { get; set; }=default!;
}
