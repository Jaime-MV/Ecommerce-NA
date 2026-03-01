namespace Ecommerce.Negocio.DTOs
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class CreateCategoriaDto
    {
        public string Nombre { get; set; } = string.Empty;
    }
}
