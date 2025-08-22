using RefactoringChallenge.Domain.Orders;

namespace RefactoringChallenge.Infrastructure.OrderLogs;

public interface IOrderLogRepository
{
    Task LogOrderCompletedAsync(Order order, CancellationToken ct = default);
    Task LogOrderOnHoldAsync(Order order, CancellationToken ct = default);
}