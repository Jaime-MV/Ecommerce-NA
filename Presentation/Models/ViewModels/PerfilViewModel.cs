using Ecommerce.Datos.Entity;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Presentacion.Models.ViewModels
{
    public class PerfilViewModel
    {
        public Usuario Usuario { get; set; } = null!;
        public List<DireccionUsuario> Direcciones { get; set; } = new();

        [Display(Name = "Número de Teléfono")]
        [StringLength(20, ErrorMessage = "El número telefónico no puede exceder los {1} caracteres.")]
        [RegularExpression(@"^\+?[0-9\s-]{8,20}$", ErrorMessage = "Por favor, introduce un número de teléfono válido.")]
        public string? NuevoTelefono { get; set; }

        public NuevaDireccionViewModel NuevaDireccion { get; set; } = new();
    }

    public class NuevaDireccionViewModel
    {
        [Required(ErrorMessage = "El nombre de contacto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre de contacto no puede exceder los {1} caracteres.")]
        [Display(Name = "Nombre de Contacto")]
        public string NombreContacto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección completa es obligatoria.")]
        [StringLength(255, ErrorMessage = "La dirección no puede exceder los {1} caracteres.")]
        [Display(Name = "Dirección Completa")]
        public string DireccionCompleta { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [StringLength(100, ErrorMessage = "La ciudad no puede exceder los {1} caracteres.")]
        public string Ciudad { get; set; } = string.Empty;

        [Display(Name = "¿Es tu dirección principal?")]
        public bool EsPrincipal { get; set; } = false;
    }
}
