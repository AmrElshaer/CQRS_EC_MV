using Application.Command.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Command.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.Number)
            .HasMaxLength(18);

        builder.HasMany(x => x.OrderItems)
            .WithOne().HasForeignKey(x => x.OrderId);

        builder.HasOne<Customer>()
            .WithMany().HasForeignKey(o => o.CustomerId);

        builder.OwnsOne(o => o.Address)
            .Property(x => x.Latitude)
            .HasColumnName("Latitude");

        builder.OwnsOne(o => o.Address)
            .Property(x => x.Longitude)
            .HasColumnName("Longitude");
    }
}
