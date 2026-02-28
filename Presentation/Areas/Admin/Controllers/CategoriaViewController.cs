using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentacion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Categorias/View")]
    public class CategoriaViewController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Categorías";
            ViewData["ActivePage"] = "Categorias";
            return View("~/Areas/Admin/Views/Categoria/Index.cshtml");
        }
    }
}
