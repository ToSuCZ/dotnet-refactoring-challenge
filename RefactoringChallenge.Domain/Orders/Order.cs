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
    
    public void ProcessOrder(decimal totalAmount, decimal discountPercent)
    {
        decimal discountAmount = totalAmount * (discountPercent / 100);
        decimal finalAmount = totalAmount - discountAmount;
            
        DiscountPercent = discountPercent;
        DiscountAmount = discountAmount;
        TotalAmount = finalAmount;
        Status = "Processed";
    }
    
    public bool HasProductsInStock()
    {
        return Items.All(item => item.Product.StockQuantity >= item.Quantity);
    }

    public void ProcessOrderItems()
    {
        foreach (OrderItem item in Items)
        {
            item.UpdateProductQuantity();
        }
    }
}
