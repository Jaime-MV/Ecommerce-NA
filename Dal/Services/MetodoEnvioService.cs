using Ecommerce.Datos.Context;
using Ecommerce.Datos.Entity;
using Ecommerce.Negocio.DTOs;
using Ecommerce.Negocio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Negocio.Services
{
    public class MetodoEnvioService : IMetodoEnvioService
    {
        private readonly ApplicationDbContext _context;

        public MetodoEnvioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MetodoEnvioDto>> ObtenerTodosAsync()
        {
            var metodos = await _context.MetodosEnvio
                                 .OrderBy(m => m.Nombre)
                                 .ToListAsync();
            return metodos.Select(m => MapToDto(m));
        }

        public async Task<MetodoEnvioDto?> ObtenerPorIdAsync(int id)
        {
            var metodo = await _context.MetodosEnvio.FindAsync(id);
            return metodo != null ? MapToDto(metodo) : null;
        }

        public async Task<MetodoEnvioDto> CrearAsync(CreateMetodoEnvioDto dto)
        {
            var metodo = new MetodoEnvio
            {
                Nombre = dto.Nombre,
                Tarifa = dto.Costo,
                TiempoEstimado = dto.TiempoEntrega ?? string.Empty
            };

            _context.MetodosEnvio.Add(metodo);
            await _context.SaveChangesAsync();
            return MapToDto(metodo);
        }

        public async Task<MetodoEnvioDto> ActualizarAsync(int id, CreateMetodoEnvioDto dto)
        {
            var metodo = await _context.MetodosEnvio.FindAsync(id);
            if (metodo == null) throw new KeyNotFoundException("Método de envío no encontrado");

            metodo.Nombre = dto.Nombre;
            metodo.Tarifa = dto.Costo;
            metodo.TiempoEstimado = dto.TiempoEntrega ?? string.Empty;

            _context.MetodosEnvio.Update(metodo);
            await _context.SaveChangesAsync();
            return MapToDto(metodo);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var metodo = await _context.MetodosEnvio.FindAsync(id);
            if (metodo == null) return false;

            _context.MetodosEnvio.Remove(metodo);
            await _context.SaveChangesAsync();
            return true;
        }

        private static MetodoEnvioDto MapToDto(MetodoEnvio m)
        {
            return new MetodoEnvioDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Costo = m.Tarifa,
                TiempoEntrega = m.TiempoEstimado
            };
        }
    }
}
