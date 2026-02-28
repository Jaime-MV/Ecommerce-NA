using Ecommerce.Negocio.DTOs;
using Ecommerce.Negocio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentacion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/categorias")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriaService.ObtenerTodasAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}", Name = "GetCategoria")]
        public async Task<IActionResult> Get(int id)
        {
            var categoria = await _categoriaService.ObtenerPorIdAsync(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevaCategoria = await _categoriaService.CrearAsync(categoriaDto);
            return CreatedAtAction(nameof(Get), new { id = nuevaCategoria.Id }, nuevaCategoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] CreateCategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var categoriaActualizada = await _categoriaService.ActualizarAsync(id, categoriaDto);
                return Ok(categoriaActualizada);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var elimino = await _categoriaService.EliminarAsync(id);
                if (!elimino) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
