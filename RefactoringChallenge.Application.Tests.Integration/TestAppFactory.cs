using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RefactoringChallenge.Domain.Customers;
using RefactoringChallenge.Domain.OrderItems;
using RefactoringChallenge.Domain.Orders;
using RefactoringChallenge.Domain.Products;
using RefactoringChallenge.Infrastructure;

namespace RefactoringChallenge.WebApi.Tests.Integration;

public class TestAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        
        builder.ConfigureServices(services =>
        {
            ServiceProvider sp = services.BuildServiceProvider();
            using IServiceScope scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();
        });
    }
    
    public static async Task ResetDatabaseAsync(ApplicationDbContext db)
    {
        await ClearAsync(db);
        await SeedAsync(db);
    }
    
    private static async Task SeedAsync(ApplicationDbContext db)
    {
        db.Customers.AddRange(
            new Customer { Id = 1, Name = "Joe Doe", Email = "joe.doe@example.com", IsVip = true, CreatedAt = new DateTime(2015,1,1, 0, 0, 0, DateTimeKind.Utc) },
            new Customer { Id = 2, Name = "John Smith", Email = "john@example.com", IsVip = false, CreatedAt = new DateTime(2023,3,15, 0, 0, 0, DateTimeKind.Utc) },
            new Customer { Id = 3, Name = "James Miller", Email = "miller@example.com", IsVip = false, CreatedAt = new DateTime(2024,1,1, 0, 0, 0, DateTimeKind.Utc) }
        );

        db.Products.AddRange(
            new Product { Id = 1, Name = "White", Category = "T-Shirts", Price = 25000, StockQuantity = 100, CreatedAt = new DateTime(2015,1,1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 2, Name = "Gray", Category = "T-Shirts", Price = 800, StockQuantity = 200, CreatedAt = new DateTime(2015,1,1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 3, Name = "Gold", Category = "T-Shirts", Price = 5000, StockQuantity = 5, CreatedAt = new DateTime(2015,1,1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 4, Name = "Black", Category = "T-Shirts", Price = 500, StockQuantity = 50, CreatedAt = new DateTime(2015,1,1, 0, 0, 0, DateTimeKind.Utc) }
        );

        db.Orders.AddRange(
            new Order { Id = 1, CustomerId = 1, CreatedAt = new DateTime(2025,4,10, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 0, Status = "Pending" },
            new Order { Id = 2, CustomerId = 1, CreatedAt = new DateTime(2025,4,12, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 0, Status = "Pending" },
            new Order { Id = 3, CustomerId = 2, CreatedAt = new DateTime(2025,4,13, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 0, Status = "Pending" },
            new Order { Id = 4, CustomerId = 3, CreatedAt = new DateTime(2025,4,14, 0, 0, 0, DateTimeKind.Utc), TotalAmount = 0, Status = "Pending" }
        );

        db.OrderItems.AddRange(
            new OrderItem { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 25000, CreatedAt = new DateTime(2015,1,1, 0, 0, 0, DateTimeKind.Utc) },
            new OrderItem { OrderId = 1, ProductId = 2, Quantity = 5, UnitPrice = 800, CreatedAt = new DateTime(2015,1,1, 0, 0, 0, DateTimeKind.Utc) },
            new OrderItem { OrderId = 2, ProductId = 4, Quantity = 3, UnitPrice = 500, CreatedAt = new DateTime(2015,1,1, 0, 0, 0, DateTimeKind.Utc) },
            new OrderItem { OrderId = 3, ProductId = 2, Quantity = 1, UnitPrice = 800, CreatedAt = new DateTime(2015,1,1, 0, 0, 0, DateTimeKind.Utc) },
            new OrderItem { OrderId = 4, ProductId = 3, Quantity = 10, UnitPrice = 5000, CreatedAt = new DateTime(2015,1,1, 0, 0, 0, DateTimeKind.Utc) }
        );

        await db.SaveChangesAsync();
    }
    
    private static async Task ClearAsync(ApplicationDbContext db)
    {
        db.Customers.RemoveRange(db.Customers);
        db.Products.RemoveRange(db.Products);
        db.Orders.RemoveRange(db.Orders);
        db.OrderItems.RemoveRange(db.OrderItems);
        db.OrderLogs.RemoveRange(db.OrderLogs);

        await db.SaveChangesAsync();
    }
}