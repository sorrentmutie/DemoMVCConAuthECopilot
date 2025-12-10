using DemoMVC.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;

namespace DemoMVCConAuthECopilot.Controllers
{
    /// <summary>
    /// Controller per la gestione dei clienti.
    /// Espone azioni per visualizzare l'elenco e i dettagli dei clienti.
    /// Accesso consentito solo agli utenti con la policy "ViewCustomers".
    /// </summary>
    [Authorize(Policy = "ViewCustomers")]
    public class CustomersController(ICustomerRepository customerRepository) : Controller
    {
        /// <summary>
        /// Restituisce la view con l'elenco dei clienti.
        /// </summary>
        /// <param name="cancellationToken">Token di cancellazione per l'operazione async.</param>
        /// <returns>View con lista di <see cref="Models.CustomerDto"/>.</returns>
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var customers = await customerRepository.GetAllAsync(cancellationToken);
            // Mapping entities to DTOs
            var customerDtos = customers.Select(c => new Models.CustomerDto
            {
                CustomerID = c.CustomerID,
                CompanyName = c.CompanyName
            }).ToList();
            return View(customerDtos);
        }

        /// <summary>
        /// Restituisce la view con i dettagli di un cliente specifico.
        /// </summary>
        /// <param name="id">ID del cliente da visualizzare.</param>
        /// <param name="cancellationToken">Token di cancellazione per l'operazione async.</param>
        /// <returns>View con <see cref="Models.CustomerDto"/> oppure errore se non trovato.</returns>
        public async Task<IActionResult> Details(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            var customer = await customerRepository.GetByIdAsync(id, cancellationToken);
            if (customer is null)
                return NotFound();

            var customerDto = new Models.CustomerDto
            {
                CustomerID = customer.CustomerID,
                CompanyName = customer.CompanyName
            };
            return View(customerDto);
        }
    }
}
