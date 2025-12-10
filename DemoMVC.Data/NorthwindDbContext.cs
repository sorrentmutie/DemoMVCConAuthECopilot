using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data
{
    public class NorthwindDbContext : DbContext
    {
        public NorthwindDbContext(DbContextOptions<NorthwindDbContext> options) : base(options) { }

        // DbSet per le principali tabelle Northwind
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
        // Aggiungi altri DbSet se necessario

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                    modelBuilder.Entity<OrderDetail>().ToTable("Order Details");
            modelBuilder.ApplyConfiguration(new Configurations.CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.OrderConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ProductConfiguration());

            // Chiave composta per OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderID, od.ProductID });

            base.OnModelCreating(modelBuilder);
        }
    }

    // Entity di esempio (da espandere secondo lo schema Northwind)
    public class Customer
    {
        public string CustomerID { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        // Altri campi...
    }

    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        // Altri campi...

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

    public class OrderDetail
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }

        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }

    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = null!;
        // Altri campi...
    }
}
