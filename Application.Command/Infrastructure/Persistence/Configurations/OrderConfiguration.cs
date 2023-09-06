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

        builder.HasOne<Customer>()
            .WithMany().HasForeignKey(o => o.CustomerId);

        builder.HasMany(o => o.OrderItems)
            .WithOne().HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(o=>o.OrderItems)
            .WithOne().HasForeignKey(i=>i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(o => o.Address)
            .Property(x => x.Latitude)
            .HasColumnName("Latitude");

        builder.OwnsOne(o => o.Address)
            .Property(x => x.Longitude)
            .HasColumnName("Longitude");
    }
}
