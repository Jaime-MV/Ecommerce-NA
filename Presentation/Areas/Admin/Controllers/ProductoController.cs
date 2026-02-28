using Ecommerce.Negocio.DTOs;
using Ecommerce.Negocio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentacion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/productos")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.ObtenerTodosConCategoriaAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var producto = await _productoService.ObtenerPorIdAsync(id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductoDto productoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevoProducto = await _productoService.CrearAsync(productoDto);
            return CreatedAtAction(nameof(Get), new { id = nuevoProducto.Id }, nuevoProducto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] CreateProductoDto productoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var productoActualizado = await _productoService.ActualizarAsync(id, productoDto);
                return Ok(productoActualizado);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        [HttpPatch("{id}/descuento")]
        public async Task<IActionResult> AplicarDescuento(int id, [FromBody] int porcentaje)
        {
            try
            {
                var exito = await _productoService.AplicarDescuentoAsync(id, porcentaje);
                if (!exito) return NotFound();
                return Ok(new { message = "Descuento aplicado correctamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var elimino = await _productoService.EliminarAsync(id);
            if (!elimino) return NotFound();
            return NoContent();
        }
    }
}
