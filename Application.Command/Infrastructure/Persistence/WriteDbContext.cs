using System.Reflection;
using Application.Command.Features.Customers.Entities;
using Application.Command.Features.Orders.Entities;
using Application.Command.Features.Products.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Command.Infrastructure.Persistence;

public class WriteDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; } = default!;

    public DbSet<Customer> Customers { get; set; } = default!;

    public DbSet<Product> Products { get; set; } = default!;

    public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Customer>()
            .HasData(new List<Customer>()
            {
                new(CustomerId.From(Guid.NewGuid()), "amr"),
                new(CustomerId.From(Guid.NewGuid()), "fares")
            });

        base.OnModelCreating(modelBuilder);
    }
}
