using RefactoringChallenge.Domain.Orders;

namespace RefactoringChallenge.Application.Orders;

public interface ICustomerOrderProcessor
{
    Task<List<Order>> ProcessCustomerOrdersAsync(int customerId, CancellationToken ct = default);
}