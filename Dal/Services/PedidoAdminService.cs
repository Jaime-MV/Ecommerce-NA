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

        // Mapeo: DB int → string legible para el frontend
        private static readonly Dictionary<int, string> EstadoMap = new()
        {
            { 0, "Creado" },
            { 1, "Pagado" },
            { 2, "Enviado" },
            { 3, "Completado" },
            { 4, "Cancelado" }
        };

        private static readonly Dictionary<string, int> EstadoReverseMap = 
            EstadoMap.ToDictionary(kv => kv.Value, kv => kv.Key);

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
            if (!EstadoReverseMap.ContainsKey(nuevoEstado))
            {
                throw new ArgumentException($"Estado no válido: '{nuevoEstado}'. Valores válidos: {string.Join(", ", EstadoReverseMap.Keys)}");
            }

            var pedido = await _context.Pedidos.FindAsync(pedidoId);
            if (pedido == null) throw new KeyNotFoundException("Pedido no encontrado.");

            pedido.Estado = EstadoReverseMap[nuevoEstado];
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
            return MapToDto(pedido);
        }

        public async Task<bool> CancelarPedidoAsync(int pedidoId)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);
            if (pedido == null) return false;

            if (pedido.Estado == 3 || pedido.Estado == 4) // Completado o Cancelado
            {
                 throw new InvalidOperationException("El pedido ya está completado o cancelado y no puede modificarse.");
            }

            pedido.Estado = 4; // Cancelado
            
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
            return true;
        }

        private static PedidoDto MapToDto(Pedido p)
        {
            return new PedidoDto
            {
                Id = p.Id,
                FechaPedido = p.FechaPedido,
                Estado = EstadoMap.GetValueOrDefault(p.Estado, "Desconocido"),
                UsuarioId = p.UsuarioId,
                Total = p.Total,
                CostoEnvio = p.CostoEnvioPagado,
                DireccionEntrega = p.DireccionEnvioSnapshot
            };
        }
    }
}
