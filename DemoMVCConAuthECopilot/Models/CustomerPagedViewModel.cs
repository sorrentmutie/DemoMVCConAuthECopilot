using System.Collections.Generic;

namespace DemoMVCConAuthECopilot.Models
{
    /// <summary>
    /// ViewModel per la paginazione dei clienti.
    /// </summary>
    public class CustomerPagedViewModel
    {
        public IEnumerable<CustomerDto> Customers { get; set; } = new List<CustomerDto>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}