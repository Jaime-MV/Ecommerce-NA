using Ecommerce.Negocio.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Negocio.Interfaces
{
    public interface IPedidoAdminService
    {
        Task<IEnumerable<PedidoDto>> ObtenerTodosLosPedidosAsync();
        Task<PedidoDto?> ObtenerDetallePedidoAsync(int pedidoId);
        Task<PedidoDto> ActualizarEstadoPedidoAsync(int pedidoId, string nuevoEstado);
        Task<bool> CancelarPedidoAsync(int pedidoId);
    }
}
