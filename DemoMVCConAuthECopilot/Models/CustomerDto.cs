using System.ComponentModel.DataAnnotations;

namespace DemoMVCConAuthECopilot.Models
{
    public class CustomerDto
    {
        [Required]
        public string CustomerID { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;
    }
}