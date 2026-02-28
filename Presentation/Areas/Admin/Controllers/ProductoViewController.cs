using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentacion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Productos/View")]
    public class ProductoViewController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Productos";
            ViewData["ActivePage"] = "Productos";
            return View("~/Areas/Admin/Views/Producto/Index.cshtml");
        }
    }
}
