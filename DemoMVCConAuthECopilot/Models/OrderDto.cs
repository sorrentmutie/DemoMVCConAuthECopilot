using System;

namespace DemoMVCConAuthECopilot.Models
{
    /// <summary>
    /// DTO per la visualizzazione di un ordine.
    /// </summary>
    public class OrderDto
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}