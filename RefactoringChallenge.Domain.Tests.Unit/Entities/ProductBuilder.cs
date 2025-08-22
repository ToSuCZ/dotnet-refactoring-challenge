using RefactoringChallenge.Domain.Products;

namespace RefactoringChallenge.Domain.Tests.Unit.Entities;

public class ProductBuilder
{
    private int _stockQuantity = 0;

    public ProductBuilder WithStockQuantity(int qty)
    {
        _stockQuantity = qty;
        return this;
    }

    public Product Build()
    {
        return new Product
        {
            StockQuantity = _stockQuantity
        };
    }
}