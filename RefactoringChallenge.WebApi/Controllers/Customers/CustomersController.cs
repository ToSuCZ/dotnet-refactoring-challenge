using Microsoft.AspNetCore.Mvc;
using RefactoringChallenge.Application.Orders;
using RefactoringChallenge.Domain.Orders;

namespace RefactoringChallenge.WebApi.Controllers.Customers;

[ApiController]
[Route("api/v{apiVersion:apiVersion}/[controller]")]
public class CustomersController(ICustomerOrderProcessor customerOrderProcessor) : ControllerBase
{
    [HttpPost("process-orders")]
    public async Task<IActionResult> ProcessOrders(ProcessCustomerOrdersRequest request, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) {
            return ValidationProblem(ModelState);
        }
        
        List<Order> orders = await customerOrderProcessor.ProcessCustomerOrdersAsync(request.CustomerId, ct);
        
        return Ok(orders);
    }
}