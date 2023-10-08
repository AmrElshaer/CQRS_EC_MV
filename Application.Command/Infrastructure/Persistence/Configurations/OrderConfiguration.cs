﻿using Application.Command.Features.Customers.Entities;
using Application.Command.Features.Orders.Entities;
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


        builder.OwnsOne(o => o.Location)
            .Property(x => x.Latitude)
            .HasColumnName("Latitude");

        builder.OwnsOne(o => o.Location)
            .Property(x => x.Longitude)
            .HasColumnName("Longitude");
    }
}
