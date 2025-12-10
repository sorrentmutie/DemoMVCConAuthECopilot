namespace DemoMVC.Data.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<OrderWithTotalAmount>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Order?> GetByIdAsync(int orderId, CancellationToken cancellationToken = default);
}

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default);
}
