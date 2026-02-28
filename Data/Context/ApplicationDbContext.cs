using Ecommerce.Datos.Entity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Datos.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductoVariante> ProductoVariantes { get; set; }
        public DbSet<MetodoEnvio> MetodosEnvio { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales y restricciones únicas

            modelBuilder.Entity<ProductoVariante>()
                .HasIndex(pv => new { pv.ProductoId, pv.Talla, pv.Color })
                .IsUnique();

        }
    }
}
