using System.ComponentModel.DataAnnotations;

namespace RefactoringChallenge.WebApi.Controllers.Customers;

public record ProcessCustomerOrdersRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be a positive integer.")]
    public int CustomerId { get; init; }
}