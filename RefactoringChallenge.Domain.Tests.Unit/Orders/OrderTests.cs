using RefactoringChallenge.Domain.OrderItems;
using RefactoringChallenge.Domain.Orders;
using RefactoringChallenge.Domain.Products;
using RefactoringChallenge.Domain.Tests.Unit.Entities;
using Shouldly;

namespace RefactoringChallenge.Domain.Tests.Unit.Orders;

public class OrderTests
{
    [Fact]
    public void ProcessOrder_sets_all_amounts_and_status()
    {
        // Arrange
        Order order = new OrderBuilder().Build();

        // Act
        order.ProcessOrder(totalAmount: 1000m, discountPercent: 12.5m);

        // Assert
        order.DiscountPercent.ShouldBe(12.5m);
        order.DiscountAmount.ShouldBe(125m);
        order.TotalAmount.ShouldBe(875m);
        order.Status.ShouldBe("Processed");
    }

    [Fact]
    public void ProcessOrder_with_zero_discount_keeps_total_equal_and_amount_zero()
    {
        // Arrange
        Order order = new OrderBuilder().Build();

        // Act
        order.ProcessOrder(500m, 0m);

        // Assert
        order.DiscountAmount.ShouldBe(0m);
        order.TotalAmount.ShouldBe(500m);
        order.Status.ShouldBe("Processed");
    }

    [Fact]
    public void HasProductsInStock_returns_true_when_all_items_have_enough_stock()
    {
        // Arrange
        Product p1 = new ProductBuilder().WithStockQuantity(10).Build();
        Product p2 = new ProductBuilder().WithStockQuantity(5).Build();

        OrderItem i1 = new OrderItemBuilder().WithProduct(p1).WithQuantity(3).Build();
        OrderItem i2 = new OrderItemBuilder().WithProduct(p2).WithQuantity(5).Build();

        Order order = new OrderBuilder().WithItems(i1, i2).Build();

        // Act
        bool ok = order.HasProductsInStock();

        // Assert
        ok.ShouldBeTrue();
    }

    [Fact]
    public void HasProductsInStock_returns_false_when_any_item_exceeds_stock()
    {
        // Arrange
        Product p1 = new ProductBuilder().WithStockQuantity(2).Build();
        Product p2 = new ProductBuilder().WithStockQuantity(1).Build();

        OrderItem i1 = new OrderItemBuilder().WithProduct(p1).WithQuantity(2).Build();
        OrderItem i2 = new OrderItemBuilder().WithProduct(p2).WithQuantity(3).Build();

        // Act
        Order order = new OrderBuilder().WithItems(i1, i2).Build();

        order.HasProductsInStock().ShouldBeFalse();
    }

    [Fact]
    public void HasProductsInStock_boundary_equal_quantity_is_true()
    {
        // Arrange
        Product p = new ProductBuilder().WithStockQuantity(7).Build();
        OrderItem item = new OrderItemBuilder().WithProduct(p).WithQuantity(7).Build();

        // Act
        Order order = new OrderBuilder().WithItems(item).Build();

        // Assert
        order.HasProductsInStock().ShouldBeTrue();
    }
}