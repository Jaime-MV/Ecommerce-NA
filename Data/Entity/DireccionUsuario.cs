using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Datos.Entity
{
    [Table("DireccionUsuario")]
    public class DireccionUsuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string UsuarioId { get; set; } = null!;

        [StringLength(100)]
        public string? NombreContacto { get; set; }

        [Required]
        [StringLength(255)]
        public string DireccionCompleta { get; set; } = null!;

        [StringLength(100)]
        public string? Ciudad { get; set; }

        public bool EsPrincipal { get; set; } = false;

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } = null!;
    }
}
