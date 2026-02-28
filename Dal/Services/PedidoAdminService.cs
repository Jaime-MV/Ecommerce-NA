using Ecommerce.Datos.Context;
using Ecommerce.Datos.Entity;
using Ecommerce.Negocio.DTOs;
using Ecommerce.Negocio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Negocio.Services
{
    public class PedidoAdminService : IPedidoAdminService
    {
        private readonly ApplicationDbContext _context;

        public PedidoAdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidoDto>> ObtenerTodosLosPedidosAsync()
        {
            var pedidos = await _context.Pedidos
                                 .Include(p => p.MetodoEnvio)
                                 .OrderByDescending(p => p.FechaPedido)
                                 .ToListAsync();
            return pedidos.Select(p => MapToDto(p));
        }

        public async Task<PedidoDto?> ObtenerDetallePedidoAsync(int pedidoId)
        {
            var pedido = await _context.Pedidos
                                 .Include(p => p.MetodoEnvio)
                                 .FirstOrDefaultAsync(p => p.Id == pedidoId);
            return pedido != null ? MapToDto(pedido) : null;
        }

        public async Task<PedidoDto> ActualizarEstadoPedidoAsync(int pedidoId, string nuevoEstado)
        {
            var validStates = new List<string> { "Creado", "Pagado", "Enviado", "Completado", "Cancelado" };
            if (!validStates.Contains(nuevoEstado))
            {
                throw new ArgumentException("Estado no válido.");
            }

            var pedido = await _context.Pedidos.FindAsync(pedidoId);
            if (pedido == null) throw new KeyNotFoundException("Pedido no encontrado.");

            pedido.Estado = nuevoEstado;
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
            return MapToDto(pedido);
        }

        public async Task<bool> CancelarPedidoAsync(int pedidoId)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);
            if (pedido == null) return false;

            if (pedido.Estado == "Completado" || pedido.Estado == "Cancelado")
            {
                 throw new InvalidOperationException("El pedido ya está completado o cancelado y no puede modificarse.");
            }

            pedido.Estado = "Cancelado";
            
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
            return true;
        }

        private static PedidoDto MapToDto(Pedido p)
        {
            return new PedidoDto
            {
                Id = p.Id,
                FechaCreacion = p.FechaPedido,
                Estado = p.Estado,
                UsuarioId = string.Empty, // TODO: No implementado en la entidad aún
                Total = p.Total,
                CostoEnvio = p.CostoEnvioPagado,
                DireccionEntrega = string.Empty // TODO: No implementado en la entidad aún
            };
        }
    }
}
