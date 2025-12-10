
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data.Repositories;

public class OrderWithTotalAmount
{
    public int OrderID { get; set; }
    public string CustomerID { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
}

public class OrderRepository(NorthwindDbContext db) : IOrderRepository
{
    public async Task<IEnumerable<OrderWithTotalAmount>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var orders = await db.Orders
            .Include(o => o.OrderDetails)
            .AsNoTracking()
            .Where(o => o.CustomerID == customerId)
            .ToListAsync(cancellationToken);

        var result = orders.Select(order => new OrderWithTotalAmount
        {
            OrderID = order.OrderID,
            CustomerID = order.CustomerID,
            OrderDate = order.OrderDate,
            TotalAmount = order.OrderDetails.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))
        });

        return result;
    }

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
