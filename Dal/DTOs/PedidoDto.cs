namespace Ecommerce.Negocio.DTOs
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string UsuarioId { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public decimal CostoEnvio { get; set; }
        public string DireccionEntrega { get; set; } = string.Empty;
    }
}
