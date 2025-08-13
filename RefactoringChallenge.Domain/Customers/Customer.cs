using RefactoringChallenge.Domain.Common;
using RefactoringChallenge.Domain.Orders;

namespace RefactoringChallenge.Domain.Customers;

public class Customer : EntityBase
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsVip { get; set; }
    
    public List<Order> Orders { get; set; } = [];
}
