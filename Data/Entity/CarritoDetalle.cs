using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Datos.Entity
{
    [Table("CarritoDetalle")]
    public class CarritoDetalle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CarritoId { get; set; }

        [Required]
        public int ProductoVarianteId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [ForeignKey("CarritoId")]
        public Carrito Carrito { get; set; } = null!;

        [ForeignKey("ProductoVarianteId")]
        public ProductoVariante ProductoVariante { get; set; } = null!;
    }
}
