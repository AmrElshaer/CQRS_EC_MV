using Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Infrastructure.Persistence.Configurations;

public class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.Number)
            .HasMaxLength(18);
        builder.HasMany(x => x.OrderItems)
            .WithOne().HasForeignKey(x => x.OrderId);

        builder.HasOne<Customer>()
            .WithMany().HasForeignKey(o => o.CustomerId);

    }
}
