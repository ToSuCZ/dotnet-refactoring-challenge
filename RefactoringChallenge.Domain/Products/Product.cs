using RefactoringChallenge.Domain.Common;

namespace RefactoringChallenge.Domain.Products;

public class Product : EntityBase
{
    public string Name { get; set; } = null!;
    public string Category { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}
