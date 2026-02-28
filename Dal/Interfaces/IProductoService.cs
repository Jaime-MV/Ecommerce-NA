using Ecommerce.Negocio.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Negocio.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDto>> ObtenerTodosConCategoriaAsync();
        Task<ProductoDto?> ObtenerPorIdAsync(int id);
        Task<ProductoDto> CrearAsync(CreateProductoDto productoDto);
        Task<ProductoDto> ActualizarAsync(int id, CreateProductoDto productoDto);
        Task<bool> AplicarDescuentoAsync(int productoId, int porcentaje);
        Task<bool> EliminarAsync(int id);
    }
}
