using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RefactoringChallenge.Infrastructure.Customers;
using RefactoringChallenge.Infrastructure.OrderItems;
using RefactoringChallenge.Infrastructure.OrderLogs;
using RefactoringChallenge.Infrastructure.Orders;
using RefactoringChallenge.Infrastructure.Products;

namespace RefactoringChallenge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment env)
    {
        services.AddDbContext<ApplicationDbContext>(o =>
        {
            if (env.IsEnvironment("Test"))
            {
                o.UseInMemoryDatabase($"testdb_{Guid.NewGuid()}");
            }
            else
            {
                services.AddSqlServer<ApplicationDbContext>(
                    configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsAssembly("RefactoringChallenge.Infrastructure.Migrations"));
            }
        });


        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IOrderLogRepository, OrderLogRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}