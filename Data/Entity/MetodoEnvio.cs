using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Datos.Entity
{
    public class MetodoEnvio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del método de envío es requerido")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tarifa { get; set; }

        [Required]
        [StringLength(100)]
        public string TiempoEstimado { get; set; } = null!;

        public bool Activo { get; set; } = true;
    }
}
