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
    public class ProductoVarianteService : IProductoVarianteService
    {
        private readonly ApplicationDbContext _context;

        public ProductoVarianteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoVarianteDto>> ObtenerVariantesPorProductoAsync(int productoId)
        {
            var variantes = await _context.ProductoVariantes
                                 .Where(v => v.ProductoId == productoId)
                                 .ToListAsync();
            return variantes.Select(v => MapToDto(v));
        }

        public async Task<ProductoVarianteDto> AgregarVarianteAsync(CreateProductoVarianteDto dto)
        {
            var varianteExistente = await _context.ProductoVariantes
                .FirstOrDefaultAsync(v => v.ProductoId == dto.ProductoId && 
                                          v.Talla.ToLower() == dto.Talla.ToLower() && 
                                          v.Color != null && dto.Color != null &&
                                          v.Color.ToLower() == dto.Color.ToLower());

            if (varianteExistente != null)
            {
                throw new InvalidOperationException("Ya existe una variante con esta talla y color para el producto.");
            }

            var variante = new ProductoVariante
            {
                ProductoId = dto.ProductoId,
                Talla = dto.Talla,
                Color = dto.Color,
                Stock = dto.Stock
            };

            _context.ProductoVariantes.Add(variante);
            await _context.SaveChangesAsync();
            return MapToDto(variante);
        }

        public async Task<ProductoVarianteDto> ActualizarStockAsync(int varianteId, int nuevoStock)
        {
            if (nuevoStock < 0)
                throw new ArgumentException("El stock no puede ser negativo.");

            var variante = await _context.ProductoVariantes.FindAsync(varianteId);
            if (variante == null)
            {
                 throw new KeyNotFoundException("Variante no encontrada.");
            }

            variante.Stock = nuevoStock;
            _context.ProductoVariantes.Update(variante);
            await _context.SaveChangesAsync();
            return MapToDto(variante);
        }

        public async Task<bool> DesactivarVarianteAsync(int id)
        {
            var variante = await _context.ProductoVariantes.FindAsync(id);
            if (variante == null) return false;

            // Sin campo Activo en DB, eliminamos el registro directamente
            _context.ProductoVariantes.Remove(variante);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ProductoVarianteDto MapToDto(ProductoVariante v)
        {
            return new ProductoVarianteDto
            {
                Id = v.Id,
                Talla = v.Talla,
                Color = v.Color,
                Stock = v.Stock,
                ProductoId = v.ProductoId
            };
        }
    }
}
