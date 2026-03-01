using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Datos.Entity
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [StringLength(450)]
        public string Id { get; set; } = null!;

        [StringLength(256)]
        public string? UserName { get; set; }

        [StringLength(256)]
        public string? NormalizedUserName { get; set; }

        [StringLength(256)]
        public string? Email { get; set; }

        [StringLength(256)]
        public string? NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        [StringLength(100)]
        public string? NombreCompleto { get; set; }

        [StringLength(20)]
        public string? PreferenciaCompra { get; set; }
    }
}
