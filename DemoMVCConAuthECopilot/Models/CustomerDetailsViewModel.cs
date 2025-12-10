using System.Collections.Generic;

namespace DemoMVCConAuthECopilot.Models
{
    /// <summary>
    /// ViewModel per la pagina di dettaglio cliente con ordini.
    /// </summary>
    public class CustomerDetailsViewModel
    {
        public CustomerDto Customer { get; set; } = new CustomerDto();
        public IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}