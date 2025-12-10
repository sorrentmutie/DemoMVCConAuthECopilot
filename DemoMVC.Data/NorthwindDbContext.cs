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
        // Aggiungi altri DbSet se necessario

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.OrderConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.ProductConfiguration());
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
    }

    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = null!;
        // Altri campi...
    }
}
