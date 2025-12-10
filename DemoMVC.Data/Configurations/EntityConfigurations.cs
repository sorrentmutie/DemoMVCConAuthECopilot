using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoMVC.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.CustomerID);
        builder.Property(c => c.CustomerID).HasMaxLength(5).IsRequired();
        builder.Property(c => c.CompanyName).HasMaxLength(40).IsRequired();
        // Altre configurazioni...
    }
}

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.OrderID);
        builder.Property(o => o.OrderDate);
        // Altre configurazioni...
    }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.ProductID);
        builder.Property(p => p.ProductName).HasMaxLength(40).IsRequired();
        // Altre configurazioni...
    }
}
