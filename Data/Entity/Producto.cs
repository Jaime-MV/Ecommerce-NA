using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Datos.Entity
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(150)]
        public string Nombre { get; set; } = null!;

        [Required]
        public string Descripcion { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioBase { get; set; }

        public int PorcentajeDescuento { get; set; } = 0;

        public string? ImagenUrl { get; set; }

        public bool Activo { get; set; } = true;

        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; } = null!;

        public ICollection<ProductoVariante> Variantes { get; set; } = new List<ProductoVariante>();
    }
}
