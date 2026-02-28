using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Datos.Entity
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        public DateTime FechaPedido { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = "Creado"; // ej: Creado, Pagado, Enviado, Completado, Cancelado

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public int MetodoEnvioId { get; set; }

        [ForeignKey("MetodoEnvioId")]
        public MetodoEnvio MetodoEnvio { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoEnvioPagado { get; set; }
        
        // TODO: En un escenario completo, aquí irían relaciones hacia Usuario y DetallesPedido
    }
}
