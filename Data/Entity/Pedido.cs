using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Datos.Entity
{
    [Table("Pedido")]
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string UsuarioId { get; set; } = null!;

        [Required]
        public int MetodoEnvioId { get; set; }

        [ForeignKey("MetodoEnvioId")]
        public MetodoEnvio MetodoEnvio { get; set; } = null!;

        public DateTime FechaPedido { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        public int Estado { get; set; } = 0; // 0=Creado, 1=Pagado, 2=Enviado, 3=Completado, 4=Cancelado

        [StringLength(500)]
        public string? DireccionEnvioSnapshot { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoEnvioPagado { get; set; }
    }
}
