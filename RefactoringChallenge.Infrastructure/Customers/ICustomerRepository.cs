using RefactoringChallenge.Domain.Customers;

namespace RefactoringChallenge.Infrastructure.Customers;

public interface ICustomerRepository
{
    Task<Customer?> GetCustomerByIdAsync(int id, CancellationToken ct = default);
}