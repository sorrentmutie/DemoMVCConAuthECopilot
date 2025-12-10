using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly NorthwindDbContext _context;

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return await _context.Customers.CountAsync(cancellationToken);
    }

    public CustomerRepository(NorthwindDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Customers
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Customer?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.CustomerID == id, cancellationToken);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);
    }
}