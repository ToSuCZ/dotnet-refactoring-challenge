using RefactoringChallenge.Domain.OrderItems;
using RefactoringChallenge.Domain.Products;

namespace RefactoringChallenge.Domain.Tests.Unit.Entities;

public class OrderItemBuilder
{
    private int _quantity = 1;
    private Product _product = new ProductBuilder().WithStockQuantity(0).Build();

    public OrderItemBuilder WithQuantity(int qty)
    {
        _quantity = qty;
        return this;
    }

    public OrderItemBuilder WithProduct(Product product)
    {
        _product = product;
        return this;
    }

    public virtual OrderItem Build()
    {
        return new OrderItem
        {
            Quantity = _quantity,
            Product = _product
        };
    }
}