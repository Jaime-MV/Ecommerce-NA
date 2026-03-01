namespace Ecommerce.Negocio.DTOs
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public DateTime FechaPedido { get; set; }
        public string Estado { get; set; } = string.Empty; // Mapeado de int a string legible
        public string UsuarioId { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public decimal CostoEnvio { get; set; }
        public string? DireccionEntrega { get; set; }
    }
}
