using RefactoringChallenge.Domain.Common;
using RefactoringChallenge.Domain.Orders;

namespace RefactoringChallenge.Domain.OrderLogs;

public class OrderLog : EntityBase
{
    public int OrderId { get; set; }
    public string Message { get; set; } = null!;
    
    public Order Order { get; set; } = null!;
}
