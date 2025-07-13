using Microsoft.AspNetCore.Mvc;
using paymentManager.DTOs;
using paymentManager.Services;

namespace paymentManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeaReturnController : ControllerBase
    {
        private readonly ITeaReturnService _teaReturnService;

        public TeaReturnController(ITeaReturnService teaReturnService)
        {
            _teaReturnService = teaReturnService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeaReturnDto>>> GetAll()
        {
            try
            {
                var teaReturns = await _teaReturnService.GetAllAsync();
                return Ok(teaReturns);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeaReturnDto>> GetById(int id)
        {
            try
            {
                var teaReturn = await _teaReturnService.GetByIdAsync(id);
                if (teaReturn == null)
                    return NotFound($"Tea return with ID {id} not found.");

                return Ok(teaReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TeaReturnDto>> Create([FromBody] CreateTeaReturnDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var teaReturn = await _teaReturnService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = teaReturn.Id }, teaReturn);
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
                var result = await _teaReturnService.DeleteAsync(id);
                if (!result)
                    return NotFound($"Tea return with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
