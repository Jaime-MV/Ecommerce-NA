namespace Ecommerce.Negocio.DTOs
{
    public class ProductoVarianteDto
    {
        public int Id { get; set; }
        public string Talla { get; set; } = string.Empty;
        public string? Color { get; set; }
        public int Stock { get; set; }
        public int ProductoId { get; set; }
    }

    public class CreateProductoVarianteDto
    {
        public int ProductoId { get; set; }
        public string Talla { get; set; } = string.Empty;
        public string? Color { get; set; }
        public int Stock { get; set; }
    }
}
