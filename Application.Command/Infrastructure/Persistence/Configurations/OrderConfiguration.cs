using Application.Command.Features.Customers.Entities;
using Application.Command.Features.Orders.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Command.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.Number)
            .HasMaxLength(18);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion<OrderIdConverter>();

        builder.Property(o => o.CustomerId)
            .HasConversion<CustomerIdConverter>();

        builder.HasOne<Customer>()
            .WithMany().HasForeignKey(o => o.CustomerId);

        builder.OwnsOne(o => o.Location)
            .Property(x => x.Latitude)
            .HasColumnName("Latitude");

        builder.OwnsOne(o => o.Location)
            .Property(x => x.Longitude)
            .HasColumnName("Longitude");
    }
}

public sealed class OrderIdConverter : ValueConverter<OrderId, Guid>
{
    public OrderIdConverter() : base(id => id.Value, guid => OrderId.From(guid)) { }
}


