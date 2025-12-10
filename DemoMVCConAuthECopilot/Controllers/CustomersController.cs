using DemoMVC.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;

namespace DemoMVCConAuthECopilot.Controllers
{
    [Authorize(Policy = "ViewCustomers")]
    public class CustomersController(ICustomerRepository customerRepository) : Controller
    {
        // GET: /Customers
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var customers = await customerRepository.GetAllAsync(cancellationToken);
            return View(customers);
        }

        // GET: /Customers/Details/{id}
        public async Task<IActionResult> Details(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            var customer = await customerRepository.GetByIdAsync(id, cancellationToken);
            if (customer is null)
                return NotFound();

            return View(customer);
        }
    }
}
