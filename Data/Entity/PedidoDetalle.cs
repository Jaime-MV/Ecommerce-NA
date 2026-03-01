using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Datos.Entity
{
    [Table("PedidoDetalle")]
    public class PedidoDetalle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }

        [Required]
        public int ProductoVarianteId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitarioPagado { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; } = null!;

        [ForeignKey("ProductoVarianteId")]
        public ProductoVariante ProductoVariante { get; set; } = null!;
    }
}
