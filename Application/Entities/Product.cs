using Application.Common;
using Application.ValueObjects;

namespace Application.Entities;

public class Product:BaseEntity
{
    public string Name { get; set; }=default!;

    public Money Price { get; set; }=default!;
}
