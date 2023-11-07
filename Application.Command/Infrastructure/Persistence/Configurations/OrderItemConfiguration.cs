using Application.Command.Features.Orders.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Command.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(x => new
        {
            x.OrderId,
            x.ProductId
        });

        builder.Property(oi => oi.ProductId)
            .HasConversion<ProductConfiguration.ProductIdConverter>();

        builder.Property(oi => oi.OrderId)
            .HasConversion<OrderIdConverter>();

        builder.HasOne<Order>()
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);
    }
}
