using Application.Command.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Command.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(100);

        builder.OwnsOne(x => x.Price)
            .Property(x => x.Amount)
            .HasPrecision(16, 4)
            .HasColumnName("Price");

        builder.OwnsOne(x => x.Price)
            .Property(x => x.CurrencyCode)
            .HasColumnName("CurrencyCode");
    }
}
