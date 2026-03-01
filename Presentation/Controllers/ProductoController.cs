using Ecommerce.Datos.Context;
using Ecommerce.Datos.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Presentacion.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public ProductoController(ApplicationDbContext context,
                                   UserManager<Usuario> userManager,
                                   SignInManager<Usuario> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ── GET /Producto/Detalle/{id} ────────────────────────────────────────
        [HttpGet]
        [Route("Producto/Detalle/{id:int}")]
        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Variantes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                return NotFound();

            // Cantidad en carrito si el usuario está logueado
            int itemsEnCarrito = 0;
            if (_signInManager.IsSignedIn(User))
            {
                var userId = _userManager.GetUserId(User)!;
                itemsEnCarrito = await _context.Carritos
                    .Where(c => c.UsuarioId == userId)
                    .SelectMany(c => c.Detalles)
                    .SumAsync(d => (int?)d.Cantidad) ?? 0;
            }

            ViewBag.ItemsEnCarrito = itemsEnCarrito;
            ViewBag.IsSignedIn = _signInManager.IsSignedIn(User);

            return View(producto);
        }
    }
}
