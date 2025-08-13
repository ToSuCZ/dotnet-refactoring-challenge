using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using RefactoringChallenge.Domain.OrderLogs;

namespace RefactoringChallenge.Infrastructure.OrderLogs;

public class OrderLogConfiguration : IEntityTypeConfiguration<OrderLog>
{
    public void Configure(EntityTypeBuilder<OrderLog> builder)
    {
        builder.ToTable("OrderLogs");
        
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        
        builder.Property(c => c.OrderId).IsRequired();
        builder.Property(c => c.Message).IsRequired().HasMaxLength(500);
        
        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnAdd();

        builder.HasIndex(c => c.Id);
        builder.HasIndex(o => o.OrderId);
        
        builder.HasOne(o => o.Order)
            .WithMany(i => i.Logs)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}