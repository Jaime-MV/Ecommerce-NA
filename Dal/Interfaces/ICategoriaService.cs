using Ecommerce.Negocio.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Negocio.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDto>> ObtenerTodasAsync();
        Task<CategoriaDto?> ObtenerPorIdAsync(int id);
        Task<CategoriaDto> CrearAsync(CreateCategoriaDto categoriaDto);
        Task<CategoriaDto> ActualizarAsync(int id, CreateCategoriaDto categoriaDto);
        Task<bool> EliminarAsync(int id);
    }
}
