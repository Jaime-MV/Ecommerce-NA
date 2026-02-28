using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentacion.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Dashboard";
            ViewData["ActivePage"] = "Dashboard";
            return View();
        }
    }
}
