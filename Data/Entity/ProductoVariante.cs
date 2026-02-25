using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Datos.Entity
{
    public class ProductoVariante
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; } = null!;

        [Required(ErrorMessage = "La talla es requerida")]
        [StringLength(20)]
        public string Talla { get; set; } = null!;

        [Required(ErrorMessage = "El color es requerido")]
        [StringLength(50)]
        public string Color { get; set; } = null!;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; } = 0;

        public bool Activo { get; set; } = true;
    }
}
