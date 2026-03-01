using Ecommerce.Negocio.DTOs;
using Ecommerce.Negocio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentacion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    [ApiController]
    public class VarianteController : ControllerBase
    {
        private readonly IProductoVarianteService _varianteService;

        public VarianteController(IProductoVarianteService varianteService)
        {
            _varianteService = varianteService;
        }

        [HttpGet("productos/{productoId}/variantes")]
        public async Task<IActionResult> GetByProducto(int productoId)
        {
            var variantes = await _varianteService.ObtenerVariantesPorProductoAsync(productoId);
            return Ok(variantes);
        }

        [HttpGet("variantes/{id}", Name = "GetVariante")]
        public async Task<IActionResult> Get(int id)
        {
            // Note: Implementation not provided in service yet
            return StatusCode(501, new { message = "No implementado" });
        }

        [HttpPost("variantes")]
        public async Task<IActionResult> Create([FromBody] CreateProductoVarianteDto varianteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var nuevaVariante = await _varianteService.AgregarVarianteAsync(varianteDto);
                return CreatedAtAction(nameof(GetByProducto), new { productoId = nuevaVariante.ProductoId }, nuevaVariante);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("variantes/{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int nuevoStock)
        {
            try
            {
                var varianteActualizada = await _varianteService.ActualizarStockAsync(id, nuevoStock);
                return Ok(varianteActualizada);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is KeyNotFoundException)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("variantes/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _varianteService.EliminarVarianteAsync(id);
            if (!eliminado) return NotFound();
            return NoContent();
        }
    }
}
