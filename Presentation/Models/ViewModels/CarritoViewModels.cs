using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Presentacion.Models.ViewModels
{
    // ViewModel para mostrar un ítem dentro del carrito
    public class CarritoItemViewModel
    {
        public int CarritoDetalleId { get; set; }
        public int ProductoVarianteId { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }
        public string Talla { get; set; } = string.Empty;
        public string? Color { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public int StockDisponible { get; set; }
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }

    // ViewModel principal del carrito
    public class CarritoViewModel
    {
        public List<CarritoItemViewModel> Items { get; set; } = new();
        public decimal Subtotal => Items.Sum(i => i.Subtotal);
        public decimal CostoEnvio { get; set; }
        public decimal Total => Subtotal + CostoEnvio;
        public int TotalItems => Items.Sum(i => i.Cantidad);
    }

    // ViewModel para agregar al carrito
    public class AgregarAlCarritoViewModel
    {
        [Required]
        public int ProductoVarianteId { get; set; }

        [Required]
        [Range(1, 99)]
        public int Cantidad { get; set; } = 1;
    }

    // ViewModel para actualizar cantidad
    public class ActualizarCantidadViewModel
    {
        [Required]
        public int CarritoDetalleId { get; set; }

        [Required]
        [Range(1, 99)]
        public int Cantidad { get; set; }
    }
}
