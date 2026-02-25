using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Datos.Entity
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es requerido")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }
        
        public bool Activo { get; set; } = true;

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
