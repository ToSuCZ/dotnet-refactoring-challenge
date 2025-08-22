using Microsoft.EntityFrameworkCore;
using RefactoringChallenge.Domain.Orders;

namespace RefactoringChallenge.Infrastructure.Orders;

public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public Task<List<Order>> GetCustomerOrdersWithStatusAsync(int customerId, string status, CancellationToken ct = default)
    {
        return dbContext.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Where(o => o.Status == status && o.CustomerId == customerId)
            .ToListAsync(ct);
    }
}