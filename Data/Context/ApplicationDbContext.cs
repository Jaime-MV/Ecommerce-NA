using Ecommerce.Datos.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Datos.Context
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Usuario is managed by IdentityDbContext (accessible via .Users)
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductoVariante> ProductoVariantes { get; set; }
        public DbSet<MetodoEnvio> MetodosEnvio { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalle> PedidoDetalles { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoDetalle> CarritoDetalles { get; set; }
        public DbSet<DireccionUsuario> DireccionesUsuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map Identity user to the existing "Usuario" table
            modelBuilder.Entity<Usuario>().ToTable("Usuario");

            // Restricción única: un carrito por usuario
            modelBuilder.Entity<Carrito>()
                .HasIndex(c => c.UsuarioId)
                .IsUnique();

            // Restricción única: variante por producto+talla+color
            modelBuilder.Entity<ProductoVariante>()
                .HasIndex(pv => new { pv.ProductoId, pv.Talla, pv.Color })
                .IsUnique();
        }
    }
}
