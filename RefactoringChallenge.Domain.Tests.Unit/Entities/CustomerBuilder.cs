using RefactoringChallenge.Domain.Customers;

namespace RefactoringChallenge.Domain.Tests.Unit.Entities;

public class CustomerBuilder
{
    private string _name = "Test Customer";
    private string _email = "test@example.com";
    private bool _isVip = false;
    private int _yearsAsCustomer = 0;

    public CustomerBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CustomerBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public CustomerBuilder IsVip(bool isVip)
    {
        _isVip = isVip;
        return this;
    }

    public CustomerBuilder WithYearsAsCustomer(int years)
    {
        _yearsAsCustomer = years;
        return this;
    }

    public Customer Build()
    {
        var customer = new Customer
        {
            Name = _name,
            Email = _email,
            IsVip = _isVip
        };

        int createdYear = DateTime.Now.Year - _yearsAsCustomer;
        customer.CreatedAt = new DateTime(createdYear, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return customer;
    }
}