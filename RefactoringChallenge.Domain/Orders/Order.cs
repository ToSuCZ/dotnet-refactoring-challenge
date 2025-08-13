using RefactoringChallenge.Domain.Common;
using RefactoringChallenge.Domain.Customers;
using RefactoringChallenge.Domain.OrderItems;
using RefactoringChallenge.Domain.OrderLogs;

namespace RefactoringChallenge.Domain.Orders;

public class Order : EntityBase
{
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public string Status { get; set; } = null!;
    
    public Customer Customer { get; set; } = null!;
    public List<OrderItem> Items { get; set; } = [];
    public List<OrderLog> Logs { get; set; } = [];
}
