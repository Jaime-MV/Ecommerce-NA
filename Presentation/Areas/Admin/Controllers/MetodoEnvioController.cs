using Ecommerce.Negocio.DTOs;
using Ecommerce.Negocio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentacion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/metodos-envio")]
    [ApiController]
    public class MetodoEnvioController : ControllerBase
    {
        private readonly IMetodoEnvioService _envioService;

        public MetodoEnvioController(IMetodoEnvioService envioService)
        {
            _envioService = envioService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var metodos = await _envioService.ObtenerTodosAsync();
            return Ok(metodos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var metodo = await _envioService.ObtenerPorIdAsync(id);
            if (metodo == null) return NotFound();
            return Ok(metodo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMetodoEnvioDto metodoEnvioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevoMetodo = await _envioService.CrearAsync(metodoEnvioDto);
            return CreatedAtAction(nameof(Get), new { id = nuevoMetodo.Id }, nuevoMetodo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] CreateMetodoEnvioDto metodoEnvioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var metodoActualizado = await _envioService.ActualizarAsync(id, metodoEnvioDto);
                return Ok(metodoActualizado);
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
            var elimino = await _envioService.EliminarAsync(id);
            if (!elimino) return NotFound();
            return NoContent();
        }
    }
}
