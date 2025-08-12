using RefactoringChallenge.Domain.Common;
using RefactoringChallenge.Domain.Products;

namespace RefactoringChallenge.Domain.OrderItems;

public class OrderItem : EntityBase
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Product Product { get; set; } = null!;
}
