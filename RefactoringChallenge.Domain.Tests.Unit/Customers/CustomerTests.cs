using RefactoringChallenge.Domain.Customers;
using RefactoringChallenge.Domain.Tests.Unit.Entities;
using Shouldly;

namespace RefactoringChallenge.Domain.Tests.Unit.Customers;

public class CustomerTests
{
    [Theory]
    [InlineData(false, 0, 0)]
    [InlineData(true,  0, 10)]
    public void Base_discounts_from_vip_and_years_only(bool isVip, int years, decimal expectedBase)
    {
        // Arrange
        Customer customer = new CustomerBuilder()
            .IsVip(isVip)
            .WithYearsAsCustomer(years)
            .Build();
        
        // Act
        decimal pct = customer.GetDiscountPercent(0m);

        // Assert
        pct.ShouldBe(expectedBase);
    }

    [Theory]
    [InlineData(0, 0)]  // < 1 year -> +0
    [InlineData(1, 0)]  // 1 year -> +0
    [InlineData(2, 2)]  // 2..4 years -> +2
    [InlineData(4, 2)]
    [InlineData(5, 5)]  // 5+ years -> +5
    [InlineData(9, 5)]
    public void Years_as_customer_tiers_are_applied(int years, decimal expectedYearsBonus)
    {
        // Arrange
        Customer customer = new CustomerBuilder()
            .WithYearsAsCustomer(years)
            .Build();
        
        // Act
        decimal pct = customer.GetDiscountPercent(0m);

        // Assert
        pct.ShouldBe(expectedYearsBonus);
    }

    [Theory]
    [InlineData(0,        0)]
    [InlineData(1000,     0)]  // not > 1000
    [InlineData(1000.01,  5)]  // > 1000 -> +5
    [InlineData(5000,     5)]  // still in >1000 tier
    [InlineData(5000.01, 10)]  // > 5000 -> +10
    [InlineData(10000,   10)]  // still in >5000 tier
    [InlineData(10000.01,15)]  // > 10000 -> +15
    public void Spend_tiers_are_applied_with_correct_boundaries(decimal totalAmount, decimal expectedSpendBonus)
    {
        // Arrange
        Customer customer = new CustomerBuilder().Build();
        
        // Act
        decimal pct = customer.GetDiscountPercent(totalAmount);

        // Assert
        pct.ShouldBe(expectedSpendBonus);
    }

    [Fact]
    public void Combines_vip_years_and_spend_until_cap()
    {
        // Arrange
        Customer customer = new CustomerBuilder()
            .IsVip(true)
            .WithYearsAsCustomer(5)
            .Build();
        
        // Act
        decimal pct = customer.GetDiscountPercent(20000m);

        // Assert
        pct.ShouldBe(25m);
    }

    [Fact]
    public void Typical_combo_without_hitting_cap()
    {
        // Arrange
        Customer customer = new CustomerBuilder()
            .IsVip(false)
            .WithYearsAsCustomer(2)
            .Build();
        
        // Act
        decimal pct = customer.GetDiscountPercent(6000m);

        // Assert
        pct.ShouldBe(12m);
    }

    [Fact]
    public void Vip_plus_mid_spend_and_mid_tenure()
    {
        // Arrange
        Customer customer = new CustomerBuilder()
            .IsVip(true)
            .WithYearsAsCustomer(4)
            .Build();
        
        // Act
        decimal pct = customer.GetDiscountPercent(1500m);

        // Assert
        pct.ShouldBe(17m);
    }

    [Fact]
    public void Lower_bound_never_negative()
    {
        // Arrange
        Customer customer = new CustomerBuilder().Build();
        
        // Act
        decimal pct = customer.GetDiscountPercent(0m);

        // Assert
        pct.ShouldBeGreaterThanOrEqualTo(0m);
    }

    [Fact]
    public void Upper_bound_capped_at_25()
    {
        // Arrange
        Customer customer = new CustomerBuilder()
            .IsVip(true)
            .WithYearsAsCustomer(10)
            .Build();
        
        // Act
        decimal pct = customer.GetDiscountPercent(10001m);

        // Assert
        pct.ShouldBe(25m);
    }
}