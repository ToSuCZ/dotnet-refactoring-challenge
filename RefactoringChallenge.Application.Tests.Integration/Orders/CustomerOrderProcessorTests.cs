using Microsoft.Data.SqlClient;
using RefactoringChallenge.Application.Orders;
using RefactoringChallenge.Domain.Entities;
using Shouldly;

namespace RefactoringChallenge.WebApi.Tests.Integration.Orders;

public class CustomerOrderProcessorTests : IAsyncLifetime
{
    private const string ConnectionString = "Server=localhost,1433;Database=refactoringchallenge;User ID=sa;Password=RCPassword1!;";

    public async Task InitializeAsync()
    {
        SetupDatabase();
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        CleanDatabase();
        await Task.CompletedTask;
    }

    [Fact]
    public void ProcessCustomerOrders_ForVipCustomerWithLargeOrder_AppliesCorrectDiscounts()
    {
        const int customerId = 1; // VIP customer
        var processor = new CustomerOrderProcessor();

        List<Order> result = processor.ProcessCustomerOrders(customerId);

        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);

        Order? largeOrder = result.Find(o => o.Id == 1);
        largeOrder.ShouldNotBeNull();
        largeOrder.DiscountPercent.ShouldBe(25); // Max. discount 25%
        largeOrder.Status.ShouldBe("Ready");

        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        using var command = new SqlCommand("SELECT StockQuantity FROM Inventory WHERE ProductId = 1", connection);
        var newStock = (int)command.ExecuteScalar();
        
        newStock.ShouldBe(90); // Origin qty 100, ordered 10
    }
    
    [Fact]
    public void ProcessCustomerOrders_ForRegularCustomerWithSmallOrder_AppliesMinimalDiscount()
    {
        const int customerId = 2; // Regular customer
        var processor = new CustomerOrderProcessor();

        List<Order> result = processor.ProcessCustomerOrders(customerId);

        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        
        Order smallOrder = result[0];
        smallOrder.DiscountPercent.ShouldBe(2); // 2% loyalty discount
        smallOrder.Status.ShouldBe("Ready");
    }
    
    [Fact]
    public void ProcessCustomerOrders_ForOrderWithUnavailableProducts_SetsOrderOnHold()
    {
        const int customerId = 3; // Customer with order with non-available items
        var processor = new CustomerOrderProcessor();

        List<Order> result = processor.ProcessCustomerOrders(customerId);

        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        
        Order onHoldOrder = result[0];
        onHoldOrder.Status.ShouldBe("OnHold");

        using var connection = new SqlConnection(ConnectionString);
        connection.Open();
        
        using var command = new SqlCommand("SELECT Message FROM OrderLogs WHERE OrderId = @OrderId ORDER BY LogDate DESC", connection);
        command.Parameters.AddWithValue("@OrderId", onHoldOrder.Id);
        var message = (string?)command.ExecuteScalar();
        
        message.ShouldNotBeNull();
        message.ShouldBe("Order on hold. Some items are not on stock.");
    }

    private static void CleanDatabase()
    {
        using var connection = new SqlConnection(ConnectionString);
        
        ExecuteNonQuery(connection, "DELETE FROM OrderLogs");
        ExecuteNonQuery(connection, "DELETE FROM OrderItems");
        ExecuteNonQuery(connection, "DELETE FROM Orders");
        ExecuteNonQuery(connection, "DELETE FROM Inventory");
        ExecuteNonQuery(connection, "DELETE FROM Products");
        ExecuteNonQuery(connection, "DELETE FROM Customers");
    }
    
    private static void SetupDatabase()
    {
        using var connection = new SqlConnection(ConnectionString);
        
        connection.Open();

        ExecuteNonQuery(connection, @"
                INSERT INTO Customers (Id, Name, Email, IsVip, RegistrationDate) VALUES 
                (1, 'Joe Doe', 'joe.doe@example.com', 1, '2015-01-01'),
                (2, 'John Smith', 'john@example.com', 0, '2023-03-15'),
                (3, 'James Miller', 'miller@example.com', 0, '2024-01-01')
            ");

        ExecuteNonQuery(connection, @"
                INSERT INTO Products (Id, Name, Category, Price) VALUES 
                (1, 'White', 'T-Shirts', 25000),
                (2, 'Gray', 'T-Shirts', 800),
                (3, 'Gold', 'T-Shirts', 5000),
                (4, 'Black', 'T-Shirts', 500)
            ");

        ExecuteNonQuery(connection, @"
                INSERT INTO Inventory (ProductId, StockQuantity) VALUES 
                (1, 100),
                (2, 200),
                (3, 5),
                (4, 50)
            ");

        ExecuteNonQuery(connection, @"
                INSERT INTO Orders (Id, CustomerId, OrderDate, TotalAmount, Status) VALUES 
                (1, 1, '2025-04-10', 0, 'Pending'),
                (2, 1, '2025-04-12', 0, 'Pending'),
                (3, 2, '2025-04-13', 0, 'Pending'),
                (4, 3, '2025-04-14', 0, 'Pending')
            ");

        ExecuteNonQuery(connection, @"
                INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES 
                (1, 1, 10, 25000),
                (1, 2, 5, 800),
                (2, 4, 3, 500),
                (3, 2, 1, 800),
                (4, 3, 10, 5000)
            ");
    }
    
    private static void ExecuteNonQuery(SqlConnection connection, string commandText)
    {
        using var command = new SqlCommand(commandText, connection);
        
        command.ExecuteNonQuery();
    }
}
