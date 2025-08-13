using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RefactoringChallenge.Domain.Orders;

namespace RefactoringChallenge.Infrastructure.Orders;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.CustomerId).IsRequired();
        builder.Property(o => o.Status).IsRequired().HasMaxLength(20);
        
        builder.Property(o => o.TotalAmount).HasPrecision(18, 2);
        builder.Property(o => o.DiscountPercent).HasPrecision(5, 2);
        builder.Property(o => o.DiscountAmount).HasPrecision(18, 2);
        
        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnAdd();

        builder.HasIndex(c => c.Id);
        builder.HasIndex(o => o.CustomerId);
        
        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}