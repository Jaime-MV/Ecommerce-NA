using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentacion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Envios/View")]
    public class MetodoEnvioViewController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Métodos de Envío";
            ViewData["ActivePage"] = "Envios";
            return View("~/Areas/Admin/Views/MetodoEnvio/Index.cshtml");
        }
    }
}
