using RefactoringChallenge.Domain.Orders;

namespace RefactoringChallenge.Infrastructure.Orders;

public interface IOrderRepository
{
    Task<List<Order>> GetCustomerOrdersWithStatusAsync(int customerId, string status, CancellationToken ct = default);
}