using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentacion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Pedidos/View")]
    public class PedidoViewController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Pedidos";
            ViewData["ActivePage"] = "Pedidos";
            return View("~/Areas/Admin/Views/Pedido/Index.cshtml");
        }
    }
}
