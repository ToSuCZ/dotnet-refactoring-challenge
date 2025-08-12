using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Domain.Customers;
using RefactoringChallenge.Domain.OrderItems;
using RefactoringChallenge.Domain.Orders;
using RefactoringChallenge.Domain.Products;

namespace RefactoringChallenge.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}