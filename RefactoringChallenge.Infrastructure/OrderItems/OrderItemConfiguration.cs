using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RefactoringChallenge.Domain.OrderItems;

namespace RefactoringChallenge.Infrastructure.OrderItems;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        
        builder.Property(c => c.OrderId).IsRequired();
        builder.Property(c => c.ProductId).IsRequired();
        
        builder.Property(c => c.Quantity).IsRequired();
        builder.Property(o => o.UnitPrice).IsRequired().HasPrecision(18, 2);
        
        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnAdd();

        builder.HasIndex(c => c.Id);
        builder.HasIndex(o => o.OrderId);
        builder.HasIndex(o => o.ProductId);
        
        builder.HasOne(o => o.Product)
            .WithMany()
            .HasForeignKey(o => o.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(o => o.Order)
            .WithMany(i => i.Items)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}