using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Datos.Entity
{
    [Table("Producto")]
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; } = null!;

        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [StringLength(255)]
        public string? ImagenUrl { get; set; }

        [StringLength(20)]
        public string? Genero { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioOriginal { get; set; }

        public int PorcentajeDescuento { get; set; } = 0;

        public DateTime? OfertaFin { get; set; }

        public ICollection<ProductoVariante> Variantes { get; set; } = new List<ProductoVariante>();
    }
}
