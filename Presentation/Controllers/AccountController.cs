using Ecommerce.Datos.Entity;
using Ecommerce.Presentacion.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Presentacion.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly Ecommerce.Datos.Context.ApplicationDbContext _context;

        public AccountController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, Ecommerce.Datos.Context.ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Usuario 
                { 
                    UserName = model.UserName, 
                    Email = model.Email,
                    NombreCompleto = model.NombreCompleto
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> MisPedidos()
        {
            var userId = _userManager.GetUserId(User)!;
            var pedidos = await _context.Pedidos
                .Include(p => p.MetodoEnvio)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.ProductoVariante)
                        .ThenInclude(v => v.Producto)
                .Where(p => p.UsuarioId == userId)
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();

            return View(pedidos);
        }

        [HttpGet]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Perfil()
        {
            var userId = _userManager.GetUserId(User)!;
            var usuario = await _userManager.FindByIdAsync(userId);
            if (usuario == null) return NotFound("Usuario no encontrado.");

            var direcciones = await _context.Set<DireccionUsuario>()
                .Where(d => d.UsuarioId == userId)
                .ToListAsync();

            var model = new PerfilViewModel
            {
                Usuario = usuario,
                Direcciones = direcciones
            };

            return View(model);
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarTelefono(PerfilViewModel vm)
        {
            var userId = _userManager.GetUserId(User)!;
            var usuario = await _userManager.FindByIdAsync(userId);

            if (usuario != null && !string.IsNullOrWhiteSpace(vm.NuevoTelefono))
            {
                var result = await _userManager.SetPhoneNumberAsync(usuario, vm.NuevoTelefono);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "No se pudo actualizar el teléfono.");
                }
            }
            
            return RedirectToAction(nameof(Perfil));
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDireccion(PerfilViewModel vm)
        {
            var userId = _userManager.GetUserId(User)!;

            ModelState.Remove("Usuario");
            ModelState.Remove("Direcciones");

            if (ModelState.IsValid)
            {
                var set = _context.Set<DireccionUsuario>();

                if (vm.NuevaDireccion.EsPrincipal)
                {
                    // Update others
                    var existings = await set.Where(d => d.UsuarioId == userId && d.EsPrincipal).ToListAsync();
                    foreach (var e in existings) e.EsPrincipal = false;
                }
                else
                {
                    // Check if there are no addresses at all, make this main if so
                    if (!await set.AnyAsync(d => d.UsuarioId == userId))
                        vm.NuevaDireccion.EsPrincipal = true;
                }

                var dir = new DireccionUsuario
                {
                    UsuarioId = userId,
                    NombreContacto = vm.NuevaDireccion.NombreContacto,
                    DireccionCompleta = vm.NuevaDireccion.DireccionCompleta,
                    Ciudad = vm.NuevaDireccion.Ciudad,
                    EsPrincipal = vm.NuevaDireccion.EsPrincipal
                };

                set.Add(dir);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Perfil));
            }

            // On failure, refill the model
            var usuario = await _userManager.FindByIdAsync(userId);
            vm.Usuario = usuario!;
            vm.Direcciones = await _context.Set<DireccionUsuario>().Where(d => d.UsuarioId == userId).ToListAsync();
            
            return View(nameof(Perfil), vm);
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EstablecerDireccionPrincipal(int id, string? returnUrl = null)
        {
            var userId = _userManager.GetUserId(User)!;
            var set = _context.Set<DireccionUsuario>();

            var addressSelected = await set.FirstOrDefaultAsync(d => d.Id == id && d.UsuarioId == userId);
            
            if (addressSelected != null)
            {
                var existings = await set.Where(d => d.UsuarioId == userId && d.EsPrincipal).ToListAsync();
                foreach (var e in existings) e.EsPrincipal = false;

                addressSelected.EsPrincipal = true;
                await _context.SaveChangesAsync();
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Perfil));
        }
    }
}
