using RefactoringChallenge.Domain.Common;
using RefactoringChallenge.Domain.OrderItems;

namespace RefactoringChallenge.Domain.Orders;

public class Order : EntityBase
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public string Status { get; set; } = null!;
    public List<OrderItem> Items { get; set; } = [];
}
