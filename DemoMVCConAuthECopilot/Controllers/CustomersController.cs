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
            /// Mostra la view per la creazione di un nuovo cliente.
            /// </summary>
            /// <returns>View di creazione cliente.</returns>
            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

            /// <summary>
            /// Gestisce la creazione di un nuovo cliente.
            /// </summary>
            /// <param name="model">DTO con i dati del cliente.</param>
            /// <param name="cancellationToken">Token di cancellazione.</param>
            /// <returns>Redirect a Index se ok, altrimenti view con errori.</returns>
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(Models.CustomerDto model, CancellationToken cancellationToken)
            {
                if (!ModelState.IsValid)
                    return View(model);

                // Mappatura DTO -> entità (assumendo esistenza di Customer)
                var customer = new DemoMVC.Data.Customer
                {
                    CustomerID = model.CustomerID,
                    CompanyName = model.CompanyName
                };

                await customerRepository.AddAsync(customer, cancellationToken);

                return RedirectToAction(nameof(Index));
            }

            /// <summary>
            /// Restituisce la view con l'elenco dei clienti.
            /// </summary>
            /// <param name="cancellationToken">Token di cancellazione per l'operazione async.</param>
            /// <returns>View con lista di <see cref="Models.CustomerDto"/>.</returns>
            public async Task<IActionResult> Index(CancellationToken cancellationToken)
            {
                // Parametri di paginazione da query string
                int pageNumber = 1;
                int pageSize = 5;
                if (Request.Query.ContainsKey("page"))
                    int.TryParse(Request.Query["page"], out pageNumber);
                if (Request.Query.ContainsKey("pageSize"))
                    int.TryParse(Request.Query["pageSize"], out pageSize);

                // Recupera i clienti paginati
                var customers = await customerRepository.GetAllAsync(pageNumber, pageSize, cancellationToken);

                // Recupera il totale (serve un nuovo metodo o proprietà, qui simulato)
                int totalCount = 0;
                if (customerRepository is DemoMVC.Data.Repositories.ICustomerRepository repoWithCount && repoWithCount is not null)
                {
                    totalCount = await repoWithCount.CountAsync(cancellationToken);
                }

                var customerDtos = customers.Select(c => new Models.CustomerDto
                {
                    CustomerID = c.CustomerID,
                    CompanyName = c.CompanyName
                }).ToList();

                var viewModel = new Models.CustomerPagedViewModel
                {
                    Customers = customerDtos,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount
                };
                return View(viewModel);
            }

            /// <summary>
            /// Restituisce la view con i dettagli di un cliente specifico e i suoi ordini.
            /// </summary>
            /// <param name="id">ID del cliente da visualizzare.</param>
            /// <param name="cancellationToken">Token di cancellazione per l'operazione async.</param>
            /// <returns>View con <see cref="CustomerDetailsViewModel"/> oppure errore se non trovato.</returns>
            public async Task<IActionResult> Details(string id, [FromServices] DemoMVC.Data.Repositories.IOrderRepository orderRepository, CancellationToken cancellationToken)
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

                // Recupera tutti gli ordini e filtra per CustomerID
                var orders = await orderRepository.GetByCustomerIdAsync(id, cancellationToken);
                var orderDtos = orders
                    .Where(o => o.CustomerID == id)
                    .Select(o => new Models.OrderDto
                    {
                        OrderID = o.OrderID,
                        OrderDate = o.OrderDate,
                        TotalAmount = o.TotalAmount
                    }).ToList();

                var viewModel = new Models.CustomerDetailsViewModel
                {
                    Customer = customerDto,
                    Orders = orderDtos
                };
                return View(viewModel);
            }
    }
}
