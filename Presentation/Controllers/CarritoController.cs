using Ecommerce.Datos.Context;
using Ecommerce.Datos.Entity;
using Ecommerce.Presentacion.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Presentacion.Controllers
{
    [Authorize] // Por ley: toda accion del carrito requiere sesion iniciada
    public class CarritoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public CarritoController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET /Carrito
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var vm = await ObtenerCarritoViewModelAsync(userId);
            return View(vm);
        }

        // POST /Carrito/Agregar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(AgregarAlCarritoViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos no validos.";
                return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index");
            }

            var userId = _userManager.GetUserId(User)!;

            // Verificar que la variante existe y tiene stock
            var variante = await _context.ProductoVariantes
                .Include(v => v.Producto)
                .FirstOrDefaultAsync(v => v.Id == model.ProductoVarianteId);

            if (variante == null)
            {
                TempData["Error"] = "Producto no encontrado.";
                return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
            }

            if (variante.Stock < model.Cantidad)
            {
                TempData["Error"] = $"Stock insuficiente. Solo hay {variante.Stock} unidades disponibles.";
                return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Detalle", "Producto", new { id = variante.ProductoId });
            }

            // Obtener o crear el carrito del usuario
            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (carrito == null)
            {
                carrito = new Carrito
                {
                    UsuarioId = userId,
                    FechaUltimaModificacion = DateTime.UtcNow
                };
                _context.Carritos.Add(carrito);
                await _context.SaveChangesAsync();
            }

            // Ya esta en el carrito?
            var detalleExistente = carrito.Detalles
                .FirstOrDefault(d => d.ProductoVarianteId == model.ProductoVarianteId);

            if (detalleExistente != null)
            {
                var nuevaCantidad = detalleExistente.Cantidad + model.Cantidad;
                if (nuevaCantidad > variante.Stock)
                {
                    TempData["Error"] = $"No puedes agregar mas. Stock disponible: {variante.Stock} uds.";
                    return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Detalle", "Producto", new { id = variante.ProductoId });
                }
                detalleExistente.Cantidad = nuevaCantidad;
            }
            else
            {
                carrito.Detalles.Add(new CarritoDetalle
                {
                    CarritoId = carrito.Id,
                    ProductoVarianteId = model.ProductoVarianteId,
                    Cantidad = model.Cantidad
                });
            }

            carrito.FechaUltimaModificacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["Exito"] = $"\"{variante.Producto.Nombre}\" agregado al carrito.";

            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index");
        }

        // POST /Carrito/Actualizar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Actualizar(ActualizarCantidadViewModel model)
        {
            var userId = _userManager.GetUserId(User)!;

            var detalle = await _context.CarritoDetalles
                .Include(d => d.Carrito)
                .Include(d => d.ProductoVariante)
                .FirstOrDefaultAsync(d => d.Id == model.CarritoDetalleId && d.Carrito.UsuarioId == userId);

            if (detalle == null)
            {
                TempData["Error"] = "Item no encontrado.";
                return RedirectToAction("Index");
            }

            if (model.Cantidad > detalle.ProductoVariante.Stock)
            {
                TempData["Error"] = $"Solo hay {detalle.ProductoVariante.Stock} unidades disponibles.";
                return RedirectToAction("Index");
            }

            detalle.Cantidad = model.Cantidad;
            detalle.Carrito.FechaUltimaModificacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // POST /Carrito/Eliminar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int carritoDetalleId)
        {
            var userId = _userManager.GetUserId(User)!;

            var detalle = await _context.CarritoDetalles
                .Include(d => d.Carrito)
                .FirstOrDefaultAsync(d => d.Id == carritoDetalleId && d.Carrito.UsuarioId == userId);

            if (detalle != null)
            {
                _context.CarritoDetalles.Remove(detalle);
                detalle.Carrito.FechaUltimaModificacion = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Producto eliminado del carrito.";
            }

            return RedirectToAction("Index");
        }

        // GET /Carrito/Checkout
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = _userManager.GetUserId(User)!;
            var vm = await ObtenerCarritoViewModelAsync(userId);

            if (!vm.Items.Any())
            {
                TempData["Error"] = "Tu carrito esta vacio.";
                return RedirectToAction("Index");
            }

            var metodosEnvio = await _context.MetodosEnvio.ToListAsync();
            ViewBag.MetodosEnvio = metodosEnvio;

            var direcciones = await _context.Set<DireccionUsuario>()
                .Where(d => d.UsuarioId == userId)
                .OrderByDescending(d => d.EsPrincipal)
                .ToListAsync();
            ViewBag.Direcciones = direcciones;

            return View(vm);
        }

        // POST /Carrito/ConfirmarPedido
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarPedido(int metodoEnvioId, string? direccion)
        {
            var userId = _userManager.GetUserId(User)!;

            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                    .ThenInclude(d => d.ProductoVariante)
                        .ThenInclude(v => v.Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (carrito == null || !carrito.Detalles.Any())
            {
                TempData["Error"] = "Tu carrito esta vacio.";
                return RedirectToAction("Index");
            }

            var metodoEnvio = await _context.MetodosEnvio.FindAsync(metodoEnvioId);
            if (metodoEnvio == null)
            {
                TempData["Error"] = "Metodo de envio no valido.";
                return RedirectToAction("Checkout");
            }

            // Calcular total
            decimal subtotal = 0;
            var detallesPedido = new List<PedidoDetalle>();

            foreach (var item in carrito.Detalles)
            {
                var variante = item.ProductoVariante;
                if (variante.Stock < item.Cantidad)
                {
                    TempData["Error"] = $"Stock insuficiente para \"{variante.Producto.Nombre}\" (talla {variante.Talla}).";
                    return RedirectToAction("Checkout");
                }

                var precioFinal = variante.Producto.PorcentajeDescuento > 0
                    ? variante.Producto.PrecioOriginal * (1 - (decimal)variante.Producto.PorcentajeDescuento / 100)
                    : variante.Producto.PrecioOriginal;

                subtotal += precioFinal * item.Cantidad;

                detallesPedido.Add(new PedidoDetalle
                {
                    ProductoVarianteId = variante.Id,
                    Cantidad = item.Cantidad,
                    PrecioUnitarioPagado = precioFinal
                });

                // Descontar stock
                variante.Stock -= item.Cantidad;
            }

            var total = subtotal + metodoEnvio.Costo;

            // Crear pedido
            var pedido = new Pedido
            {
                UsuarioId = userId,
                MetodoEnvioId = metodoEnvioId,
                FechaPedido = DateTime.UtcNow,
                Total = total,
                Estado = 0, // Creado
                CostoEnvioPagado = metodoEnvio.Costo,
                DireccionEnvioSnapshot = direccion
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            // Asociar detalles al pedido
            foreach (var det in detallesPedido)
            {
                det.PedidoId = pedido.Id;
            }
            _context.PedidoDetalles.AddRange(detallesPedido);

            // Vaciar el carrito
            _context.CarritoDetalles.RemoveRange(carrito.Detalles);
            await _context.SaveChangesAsync();

            TempData["Exito"] = $"Pedido #{pedido.Id} creado exitosamente! Total: ${total:N2}";
            return RedirectToAction("Confirmacion", new { id = pedido.Id });
        }

        // GET /Carrito/Confirmacion/{id}
        [HttpGet]
        public async Task<IActionResult> Confirmacion(int id)
        {
            var userId = _userManager.GetUserId(User)!;

            var pedido = await _context.Pedidos
                .Include(p => p.MetodoEnvio)
                .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == userId);

            if (pedido == null)
                return RedirectToAction("Index", "Home");

            return View(pedido);
        }

        // Helper privado
        private async Task<CarritoViewModel> ObtenerCarritoViewModelAsync(string userId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Detalles)
                    .ThenInclude(d => d.ProductoVariante)
                        .ThenInclude(v => v.Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == userId);

            var vm = new CarritoViewModel();

            if (carrito != null)
            {
                foreach (var detalle in carrito.Detalles)
                {
                    var producto = detalle.ProductoVariante.Producto;
                    var variante = detalle.ProductoVariante;

                    var precioFinal = producto.PorcentajeDescuento > 0
                        ? producto.PrecioOriginal * (1 - (decimal)producto.PorcentajeDescuento / 100)
                        : producto.PrecioOriginal;

                    vm.Items.Add(new CarritoItemViewModel
                    {
                        CarritoDetalleId = detalle.Id,
                        ProductoVarianteId = variante.Id,
                        ProductoId = producto.Id,
                        ProductoNombre = producto.Nombre,
                        ImagenUrl = producto.ImagenUrl,
                        Talla = variante.Talla,
                        Color = variante.Color,
                        PrecioUnitario = precioFinal,
                        Cantidad = detalle.Cantidad,
                        StockDisponible = variante.Stock
                    });
                }
            }

            return vm;
        }
    }
}
