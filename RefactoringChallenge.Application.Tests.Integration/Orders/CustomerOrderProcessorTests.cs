using Microsoft.Extensions.DependencyInjection;
using RefactoringChallenge.Application.Orders;
using RefactoringChallenge.Domain.OrderLogs;
using RefactoringChallenge.Domain.Orders;
using RefactoringChallenge.Domain.Products;
using RefactoringChallenge.Infrastructure;
using Shouldly;

namespace RefactoringChallenge.WebApi.Tests.Integration.Orders;

public class CustomerOrderProcessorTests : IAsyncLifetime
{
    private readonly TestAppFactory _factory = new();
    private IServiceScope _scope = null!;
    private ICustomerOrderProcessor _sut = null!;
    private ApplicationDbContext _db = null!;

    public async Task InitializeAsync()
    {
        _scope = _factory.Services.CreateScope();
        _sut = _scope.ServiceProvider.GetRequiredService<ICustomerOrderProcessor>();
        _db  = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await TestAppFactory.ResetDatabaseAsync(_db);
        
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        _scope.Dispose();
        await _factory.DisposeAsync();
    }

    [Fact]
    public async Task ProcessCustomerOrders_ForVipCustomerWithLargeOrder_AppliesCorrectDiscounts()
    {
        // Arrange
        const int customerId = 1; // VIP customer
        
        // Act
        List<Order> result = await _sut.ProcessCustomerOrdersAsync(customerId);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);

        Order? largeOrder = result.Find(o => o.Id == 1);
        largeOrder.ShouldNotBeNull();
        largeOrder.DiscountPercent.ShouldBe(25); // Max. discount 25%
        largeOrder.Status.ShouldBe("Ready");

        Product modifiedProduct = _db.Products.First(x => x.Id == 1);
        modifiedProduct.StockQuantity.ShouldBe(90); // Origin qty 100, ordered 10
    }
    
    [Fact]
    public async Task ProcessCustomerOrders_ForRegularCustomerWithSmallOrder_AppliesMinimalDiscount()
    {
        // Arrange
        const int customerId = 2; // Regular customer

        // Act
        List<Order> result = await _sut.ProcessCustomerOrdersAsync(customerId);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        
        Order smallOrder = result[0];
        smallOrder.DiscountPercent.ShouldBe(2); // 2% loyalty discount
        smallOrder.Status.ShouldBe("Ready");
    }
    
    [Fact]
    public async Task ProcessCustomerOrders_ForOrderWithUnavailableProducts_SetsOrderOnHold()
    {
        // Arrange
        const int customerId = 3; // Customer with order with non-available items
        
        // Act
        List<Order> result = await _sut.ProcessCustomerOrdersAsync(customerId);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        
        Order onHoldOrder = result[0];
        onHoldOrder.Status.ShouldBe("OnHold");

        OrderLog orderLog = _db.OrderLogs
            .Where(x => x.OrderId == onHoldOrder.Id)
            .OrderByDescending(x => x.CreatedAt)
            .First();
        
        orderLog.ShouldNotBeNull();
        orderLog.Message.ShouldBe("Order on hold. Some items are not on stock.");
    }
}
