using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Datos.Entity
{
    [Table("MetodoEnvio")]
    public class MetodoEnvio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del método de envío es requerido")]
        [StringLength(50)]
        public string Nombre { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Costo { get; set; }

        [StringLength(50)]
        public string? TiempoEstimado { get; set; }
    }
}
