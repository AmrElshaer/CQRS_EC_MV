using Application.Command.Features.Customers.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Command.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(100);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion<CustomerIdConverter>();
    }
}

public sealed class CustomerIdConverter : ValueConverter<CustomerId, Guid>
{
    public CustomerIdConverter() : base(id => id.Value, guid => CustomerId.From(guid)) { }
}
