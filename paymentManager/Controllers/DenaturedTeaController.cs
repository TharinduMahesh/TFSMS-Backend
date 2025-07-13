using Microsoft.AspNetCore.Mvc;
using paymentManager.DTOs;
using paymentManager.Services;

namespace paymentManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DenaturedTeaController : ControllerBase
    {
        private readonly IDenaturedTeaService _denaturedTeaService;

        public DenaturedTeaController(IDenaturedTeaService denaturedTeaService)
        {
            _denaturedTeaService = denaturedTeaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DenaturedTeaDto>>> GetAll()
        {
            try
            {
                var denaturedTeas = await _denaturedTeaService.GetAllAsync();
                return Ok(denaturedTeas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DenaturedTeaDto>> GetById(int id)
        {
            try
            {
                var denaturedTea = await _denaturedTeaService.GetByIdAsync(id);
                if (denaturedTea == null)
                    return NotFound($"Denatured tea with ID {id} not found.");

                return Ok(denaturedTea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DenaturedTeaDto>> Create([FromBody] CreateDenaturedTeaDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var denaturedTea = await _denaturedTeaService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = denaturedTea.Id }, denaturedTea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _denaturedTeaService.DeleteAsync(id);
                if (!result)
                    return NotFound($"Denatured tea with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
