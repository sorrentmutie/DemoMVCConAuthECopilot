using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoMVCConAuthECopilot.Controllers
{
    [Authorize(Policy = "AutoAziendale")]
    public class RimborsoBenzinaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
