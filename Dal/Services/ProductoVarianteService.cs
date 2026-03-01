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
            // Validate input lengths to match DB constraints
            if (string.IsNullOrWhiteSpace(dto.Talla))
                throw new InvalidOperationException("La talla es requerida.");

            if (dto.Talla.Trim().Length > 10)
                throw new InvalidOperationException($"La talla no puede exceder 10 caracteres (ingresaste {dto.Talla.Trim().Length}).");

            if (!string.IsNullOrWhiteSpace(dto.Color) && dto.Color.Trim().Length > 30)
                throw new InvalidOperationException($"El color no puede exceder 30 caracteres (ingresaste {dto.Color.Trim().Length}).");

            // Normalize color: treat empty string as null
            var colorNormalized = string.IsNullOrWhiteSpace(dto.Color) ? null : dto.Color.Trim();

            // Check for duplicate using case-insensitive comparison safe for PostgreSQL
            var varianteExistente = await _context.ProductoVariantes
                .Where(v => v.ProductoId == dto.ProductoId)
                .Where(v => v.Talla.ToLower() == dto.Talla.ToLower().Trim())
                .Where(v => colorNormalized == null 
                    ? v.Color == null 
                    : v.Color != null && v.Color.ToLower() == colorNormalized.ToLower())
                .FirstOrDefaultAsync();

            if (varianteExistente != null)
            {
                throw new InvalidOperationException("Ya existe una variante con esta talla y color para el producto.");
            }

            var variante = new ProductoVariante
            {
                ProductoId = dto.ProductoId,
                Talla = dto.Talla.Trim(),
                Color = colorNormalized,
                Stock = dto.Stock
            };

            _context.ProductoVariantes.Add(variante);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Catch unique constraint violations from the DB
                if (ex.InnerException?.Message?.Contains("duplicate") == true ||
                    ex.InnerException?.Message?.Contains("unique") == true ||
                    ex.InnerException?.Message?.Contains("23505") == true)
                {
                    throw new InvalidOperationException("Ya existe una variante con esta talla y color para el producto.");
                }
                throw;
            }

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

        public async Task<bool> EliminarVarianteAsync(int id)
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
