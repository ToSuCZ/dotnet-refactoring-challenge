using RefactoringChallenge.Domain.Customers;
using RefactoringChallenge.Domain.OrderItems;
using RefactoringChallenge.Domain.Orders;

namespace RefactoringChallenge.Domain.Tests.Unit.Entities;

public class OrderBuilder
{
    private Customer _customer = new CustomerBuilder().Build();
    private List<OrderItem> _items = [];

    public OrderBuilder WithCustomer(Customer customer)
    {
        _customer = customer;
        return this;
    }

    public OrderBuilder WithItems(params OrderItem[] items)
    {
        _items = items.ToList();
        return this;
    }

    public OrderBuilder AddItem(OrderItem item)
    {
        _items.Add(item);
        return this;
    }

    public Order Build()
    {
        return new Order
        {
            Customer = _customer,
            CustomerId = 1,
            Items = _items,
            Status = "New"
        };
    }
}