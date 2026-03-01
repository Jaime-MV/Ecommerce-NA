using Ecommerce.Datos.Context;
using Ecommerce.Negocio.Interfaces;
using Ecommerce.Negocio.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyección de dependencias (BLL)
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IProductoVarianteService, ProductoVarianteService>();
builder.Services.AddScoped<IMetodoEnvioService, MetodoEnvioService>();
builder.Services.AddScoped<IPedidoAdminService, PedidoAdminService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Admin}/{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
