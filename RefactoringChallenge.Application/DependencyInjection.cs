using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RefactoringChallenge.Application.Orders;

namespace RefactoringChallenge.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ICustomerOrderProcessor, CustomerOrderProcessor>();

        return services;
    }
}