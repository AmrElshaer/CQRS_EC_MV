using Application.Command.Features.Orders.Entities;
using Application.Command.Features.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Command.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion<ProductIdConverter>();

        builder.Property(x => x.Name)
            .HasMaxLength(100);

        builder.OwnsOne(x => x.Price)
            .Property(x => x.Amount)
            .HasPrecision(16, 4)
            .HasColumnName("Price");

        builder.HasMany<OrderItem>()
            .WithOne()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(x => x.Price)
            .Property(x => x.CurrencyCode)
            .HasColumnName("CurrencyCode");
    }

    public sealed class ProductIdConverter : ValueConverter<ProductId, Guid>
    {
        public ProductIdConverter() : base(id => id.Value, guid => ProductId.From(guid)) { }
    }
    
}
