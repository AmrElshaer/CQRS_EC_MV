﻿using Application.Command.Features.Orders.Entities;
using Application.Command.Features.Products.Entities;
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
        builder.HasMany<OrderItem>()
            .WithOne()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(x => x.Price)
            .Property(x => x.CurrencyCode)
            .HasColumnName("CurrencyCode");
    }
}
