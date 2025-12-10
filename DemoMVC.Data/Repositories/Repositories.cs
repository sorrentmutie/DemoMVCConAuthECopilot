using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data.Repositories;

public class CustomerRepository(NorthwindDbContext db) : ICustomerRepository
{
    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        => await db.Customers.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Customer?> GetByIdAsync(string customerId, CancellationToken cancellationToken = default)
        => await db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.CustomerID == customerId, cancellationToken);
}

public class OrderRepository(NorthwindDbContext db) : IOrderRepository
{
    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        => await db.Orders.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Order?> GetByIdAsync(int orderId, CancellationToken cancellationToken = default)
        => await db.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.OrderID == orderId, cancellationToken);
}

public class ProductRepository(NorthwindDbContext db) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => await db.Products.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default)
        => await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductID == productId, cancellationToken);
}
