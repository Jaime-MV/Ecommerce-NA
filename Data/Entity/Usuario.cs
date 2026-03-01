using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Datos.Entity
{
    [Table("Usuario")]
    public class Usuario : IdentityUser
    {
        [StringLength(100)]
        public string? NombreCompleto { get; set; }

        [StringLength(20)]
        public string? PreferenciaCompra { get; set; }
    }
}
