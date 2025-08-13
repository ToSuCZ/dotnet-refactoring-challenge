using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RefactoringChallenge.Domain.Products;

namespace RefactoringChallenge.Infrastructure.Products;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Category).IsRequired().HasMaxLength(100);
        builder.Property(c => c.StockQuantity).IsRequired();
        builder.Property(c => c.Price).IsRequired().HasPrecision(18, 2);
        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnAdd();

        builder.HasIndex(c => c.Id);
    }
}