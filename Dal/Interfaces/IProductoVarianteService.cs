using Ecommerce.Negocio.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Negocio.Interfaces
{
    public interface IProductoVarianteService
    {
        Task<IEnumerable<ProductoVarianteDto>> ObtenerVariantesPorProductoAsync(int productoId);
        Task<ProductoVarianteDto> AgregarVarianteAsync(CreateProductoVarianteDto dto);
        Task<ProductoVarianteDto> ActualizarStockAsync(int varianteId, int nuevoStock);
        Task<bool> EliminarVarianteAsync(int id);
    }
}
