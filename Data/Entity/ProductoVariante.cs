using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Datos.Entity
{
    [Table("ProductoVariante")]
    public class ProductoVariante
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; } = null!;

        [Required(ErrorMessage = "La talla es requerida")]
        [StringLength(10)]
        public string Talla { get; set; } = null!;

        [StringLength(30)]
        public string? Color { get; set; }

        [Required]
        public int Stock { get; set; } = 0;
    }
}
