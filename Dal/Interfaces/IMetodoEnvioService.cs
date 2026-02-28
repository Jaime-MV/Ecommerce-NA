using Ecommerce.Negocio.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Negocio.Interfaces
{
    public interface IMetodoEnvioService
    {
        Task<IEnumerable<MetodoEnvioDto>> ObtenerTodosAsync();
        Task<MetodoEnvioDto?> ObtenerPorIdAsync(int id);
        Task<MetodoEnvioDto> CrearAsync(CreateMetodoEnvioDto dto);
        Task<MetodoEnvioDto> ActualizarAsync(int id, CreateMetodoEnvioDto dto);
        Task<bool> EliminarAsync(int id);
    }
}
