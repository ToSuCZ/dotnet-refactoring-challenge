using RefactoringChallenge.Domain.Customers;
using RefactoringChallenge.Domain.Orders;
using RefactoringChallenge.Infrastructure;
using RefactoringChallenge.Infrastructure.Customers;
using RefactoringChallenge.Infrastructure.OrderLogs;
using RefactoringChallenge.Infrastructure.Orders;

namespace RefactoringChallenge.Application.Orders;

public class CustomerOrderProcessor(
    ICustomerRepository customerRepository,
    IOrderRepository orderRepository,
    IOrderLogRepository orderLogRepository,
    IUnitOfWork unitOfWork) : ICustomerOrderProcessor
{
    public async Task<List<Order>> ProcessCustomerOrdersAsync(int customerId, CancellationToken ct = default)
    {
        Customer customer = await customerRepository.GetCustomerByIdAsync(customerId, ct)
                             ?? throw new KeyNotFoundException($"Zákazník s ID {customerId} nebyl nalezen.");
        
        List<Order> pendingOrders = await orderRepository.GetCustomerOrdersWithStatusAsync(customer.Id, "Pending", ct);
        List<Order> processedOrders = [];

        foreach (Order order in pendingOrders)
        {
            decimal totalAmount = order.Items.Sum(item => item.Quantity * item.UnitPrice);
            decimal discountPercent = order.Customer.GetDiscountPercent(totalAmount);
            
            order.ProcessOrder(totalAmount, discountPercent);
            await unitOfWork.SaveChangesAsync(ct);

            bool allProductsAvailable = order.HasProductsInStock();
            
            if (allProductsAvailable)
            {
                order.ProcessOrderItems();
                order.Status = "Ready";
                await unitOfWork.SaveChangesAsync(ct);

                await orderLogRepository.LogOrderCompletedAsync(order, ct);
            }
            else
            {
                order.Status = "OnHold";
                await unitOfWork.SaveChangesAsync(ct);
                
                await orderLogRepository.LogOrderOnHoldAsync(order, ct);
            }
            
            processedOrders.Add(order);
        }
        
        
        return processedOrders;
    }
}
