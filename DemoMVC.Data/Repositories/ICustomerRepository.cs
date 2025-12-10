using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DemoMVC.Data.Repositories;

public interface ICustomerRepository
{
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Customer>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Customer?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task AddAsync(Customer customer, CancellationToken cancellationToken);
}