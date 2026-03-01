using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Datos.Entity
{
    [Table("Carrito")]
    public class Carrito
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string UsuarioId { get; set; } = null!;

        public DateTime FechaUltimaModificacion { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!;

        public ICollection<CarritoDetalle> Detalles { get; set; } = new List<CarritoDetalle>();
    }
}
