using RefactoringChallenge.Domain.Common;
using RefactoringChallenge.Domain.Orders;

namespace RefactoringChallenge.Domain.Customers;

public class Customer : EntityBase
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsVip { get; set; }
    
    public List<Order> Orders { get; set; } = [];
    
    public decimal GetDiscountPercent(decimal totalAmount)
    {
        decimal discountPercent = 0;

        if (IsVip)
        {
            discountPercent += 10;
        }

        int yearsAsCustomer = DateTime.Now.Year - CreatedAt.Year;
        switch (yearsAsCustomer)
        {
            case >= 5:
                discountPercent += 5;
                break;
            case >= 2:
                discountPercent += 2;
                break;
        }

        switch (totalAmount)
        {
            case > 10000:
                discountPercent += 15;
                break;
            case > 5000:
                discountPercent += 10;
                break;
            case > 1000:
                discountPercent += 5;
                break;
        }

        return Math.Clamp(discountPercent, 0, 25);
    }
}
