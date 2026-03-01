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
    public class CategoriaService : ICategoriaService
    {
        private readonly ApplicationDbContext _context;

        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaDto>> ObtenerTodasAsync()
        {
            var categorias = await _context.Categorias
                                 .OrderBy(c => c.Nombre)
                                 .ToListAsync();

            return categorias.Select(c => MapToDto(c));
        }

        public async Task<CategoriaDto?> ObtenerPorIdAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            return categoria != null ? MapToDto(categoria) : null;
        }

        public async Task<CategoriaDto> CrearAsync(CreateCategoriaDto dto)
        {
            var categoria = new Categoria
            {
                Nombre = dto.Nombre
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return MapToDto(categoria);
        }

        public async Task<CategoriaDto> ActualizarAsync(int id, CreateCategoriaDto dto)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) throw new KeyNotFoundException("Categoría no encontrada");

            categoria.Nombre = dto.Nombre;

            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
            return MapToDto(categoria);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var categoria = await _context.Categorias
                                          .Include(c => c.Productos)
                                          .FirstOrDefaultAsync(c => c.Id == id);
            
            if (categoria == null) return false;

            if (categoria.Productos != null && categoria.Productos.Any())
            {
                throw new InvalidOperationException("No se puede eliminar la categoría porque tiene productos asociados.");
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }

        private static CategoriaDto MapToDto(Categoria c)
        {
            return new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre
            };
        }
    }
}
