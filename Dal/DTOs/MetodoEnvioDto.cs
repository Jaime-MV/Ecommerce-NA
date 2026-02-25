namespace Ecommerce.Negocio.DTOs
{
    public class MetodoEnvioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Costo { get; set; }
        public string? TiempoEntrega { get; set; }
    }

    public class CreateMetodoEnvioDto
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Costo { get; set; }
        public string? TiempoEntrega { get; set; }
    }
}
