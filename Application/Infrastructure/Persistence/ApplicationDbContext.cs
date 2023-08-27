﻿using Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Infrastructure.Persistence;

public class ApplicationDbContext:DbContext
{
    public DbSet<Order> Orders { get; set; }=default!;

    public DbSet<Customer> Customers { get; set; }=default!;

    public DbSet<Product> Products { get; set; }=default!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    
}
