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
    public class ProductoService : IProductoService
    {
        private readonly ApplicationDbContext _context;

        public ProductoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoDto>> ObtenerTodosConCategoriaAsync()
        {
            var productos = await _context.Productos
                                 .Include(p => p.Categoria)
                                 .ToListAsync();

            return productos.Select(p => MapToDto(p));
        }

        public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
        {
            var producto = await _context.Productos
                                 .Include(p => p.Categoria)
                                 .Include(p => p.Variantes)
                                 .FirstOrDefaultAsync(p => p.Id == id);

            return producto != null ? MapToDto(producto) : null;
        }

        public async Task<ProductoDto> CrearAsync(CreateProductoDto dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                SKU = dto.SKU,
                PrecioBase = dto.PrecioBase,
                CategoriaId = dto.CategoriaId,
                ImagenUrl = dto.ImagenUrl
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return MapToDto(producto);
        }

        public async Task<ProductoDto> ActualizarAsync(int id, CreateProductoDto dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) throw new KeyNotFoundException("Producto no encontrado");

            producto.Nombre = dto.Nombre;
            producto.SKU = dto.SKU;
            producto.PrecioBase = dto.PrecioBase;
            producto.CategoriaId = dto.CategoriaId;
            producto.ImagenUrl = dto.ImagenUrl;

            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return MapToDto(producto);
        }

        public async Task<bool> AplicarDescuentoAsync(int productoId, int porcentaje)
        {
            if (porcentaje < 0 || porcentaje > 100)
                throw new ArgumentException("El porcentaje debe estar entre 0 y 100.");

            var producto = await _context.Productos.FindAsync(productoId);
            if (producto == null) return false;

            producto.PorcentajeDescuento = porcentaje;
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ProductoDto MapToDto(Producto p)
        {
            return new ProductoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                SKU = p.SKU,
                PrecioBase = p.PrecioBase,
                ImagenUrl = p.ImagenUrl,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria?.Nombre,
                PorcentajeDescuento = p.PorcentajeDescuento,
                Variantes = p.Variantes?.Select(v => new ProductoVarianteDto
                {
                    Id = v.Id,
                    Talla = v.Talla,
                    Color = v.Color,
                    Stock = v.Stock,
                    ProductoId = v.ProductoId,
                    Activo = v.Activo
                }).ToList() ?? new List<ProductoVarianteDto>()
            };
        }
    }
}
