using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Domain.Customers;

namespace RefactoringChallenge.Infrastructure.Customers;

public class CustomerRepository(ApplicationDbContext dbContext) : ICustomerRepository
{
    public Task<Customer?> GetCustomerByIdAsync(int id, CancellationToken ct = default)
    {
        return dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

}