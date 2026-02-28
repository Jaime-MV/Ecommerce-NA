using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentacion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Inventario/View")]
    public class VarianteViewController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Inventario";
            ViewData["ActivePage"] = "Inventario";
            return View("~/Areas/Admin/Views/Variante/Index.cshtml");
        }
    }
}
