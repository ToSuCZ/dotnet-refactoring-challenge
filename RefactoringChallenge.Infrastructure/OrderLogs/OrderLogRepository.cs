using RefactoringChallenge.Domain.OrderLogs;
using RefactoringChallenge.Domain.Orders;

namespace RefactoringChallenge.Infrastructure.OrderLogs;

public class OrderLogRepository(ApplicationDbContext dbContext) : IOrderLogRepository
{
    public async Task LogOrderCompletedAsync(Order order, CancellationToken ct = default)
    {
        var log = new OrderLog
        {
            OrderId = order.Id,
            Message = $"Order completed with {order.DiscountPercent}% discount. Total price: {order.TotalAmount}",
            CreatedAt = DateTime.UtcNow
        };

        await dbContext.OrderLogs.AddAsync(log, ct);
        await dbContext.SaveChangesAsync(ct);
    }
    
    public async Task LogOrderOnHoldAsync(Order order, CancellationToken ct = default)
    {
        var log = new OrderLog
        {
            OrderId = order.Id,
            Message = "Order on hold. Some items are not on stock.",
            CreatedAt = DateTime.UtcNow
        };

        await dbContext.OrderLogs.AddAsync(log, ct);
        await dbContext.SaveChangesAsync(ct);
    }
}