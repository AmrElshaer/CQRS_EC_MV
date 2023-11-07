using Application.Command.Common;

namespace Application.Command.Features.Products.Entities;

public class ProductId : EntityId
{
    private ProductId(Guid value) : base(value) { }

    public static ProductId From(Guid productId)
    {
        return new(productId);
    }
}
