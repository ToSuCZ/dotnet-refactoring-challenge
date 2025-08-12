using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RefactoringChallenge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer<ApplicationDbContext>(configuration.GetConnectionString("DefaultConnection"));

        return services;
    }
}