namespace Ecommerce.Negocio.DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioOriginal { get; set; }
        public string? ImagenUrl { get; set; }
        public string? Genero { get; set; }
        public int CategoriaId { get; set; }
        public string? CategoriaNombre { get; set; }
        public int PorcentajeDescuento { get; set; }
        public DateTime? OfertaFin { get; set; }
        public List<ProductoVarianteDto> Variantes { get; set; } = new();
    }

    public class CreateProductoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioOriginal { get; set; }
        public int CategoriaId { get; set; }
        public string? ImagenUrl { get; set; }
        public string? Genero { get; set; }
    }
}
