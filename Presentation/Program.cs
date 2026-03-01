using Ecommerce.Datos.Context;
using Ecommerce.Datos.Entity;
using Ecommerce.Negocio.Interfaces;
using Ecommerce.Negocio.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de ASP.NET Core Identity
builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{
    // Configuraciones opcionales para contraseñas, bloqueos, etc.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Inyección de dependencias (BLL)
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IProductoVarianteService, ProductoVarianteService>();
builder.Services.AddScoped<IMetodoEnvioService, MetodoEnvioService>();
builder.Services.AddScoped<IPedidoAdminService, PedidoAdminService>();

var app = builder.Build();

// ── DATA SEEDER ─────────────────────────────────────────────────────────────
// Pobla la DB con datos iniciales la primera vez que la app arranca
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        // Asegurar que las tablas existen (por si acaso EF no ha migrado aún)
        await db.Database.EnsureCreatedAsync();

        // Seed MetodoEnvio si la tabla está vacía
        if (!db.MetodosEnvio.Any())
        {
            db.MetodosEnvio.AddRange(
                new MetodoEnvio { Nombre = "Envio Estandar",      Costo = 99.00m,  TiempoEstimado = "5-7 dias habiles"   },
                new MetodoEnvio { Nombre = "Envio Express",        Costo = 199.00m, TiempoEstimado = "2-3 dias habiles"   },
                new MetodoEnvio { Nombre = "Envio Mismo Dia",      Costo = 349.00m, TiempoEstimado = "Hoy antes de 9pm"   },
                new MetodoEnvio { Nombre = "Recogida en Tienda",   Costo = 0.00m,   TiempoEstimado = "Listo en 2 horas"   }
            );
            await db.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error durante el Data Seeder al iniciar la aplicacion.");
    }
}
// ────────────────────────────────────────────────────────────────────────────

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

