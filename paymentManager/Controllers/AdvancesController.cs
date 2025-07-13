using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using paymentManager.Models;
using paymentManager.Services;
using paymentManager.DTOs;

namespace paymentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvancesController : ControllerBase
    {
        private readonly IAdvanceService _advanceService;
        private readonly ILogger<AdvancesController> _logger;

        public AdvancesController(IAdvanceService advanceService, ILogger<AdvancesController> logger)
        {
            _advanceService = advanceService;
            _logger = logger;
        }

        // GET: api/advances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvanceDTO>>> GetAdvances()
        {
            try
            {
                _logger.LogInformation("Getting all advances");
                var advances = await _advanceService.GetAllAdvancesAsync();
                return Ok(advances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all advances");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/advances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdvanceDTO>> GetAdvance(int id)
        {
            try
            {
                var advance = await _advanceService.GetAdvanceByIdAsync(id);
                if (advance == null)
                {
                    return NotFound();
                }
                return Ok(advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting advance with id {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/advances/supplier/5
        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<IEnumerable<AdvanceDTO>>> GetAdvancesBySupplier(int supplierId)
        {
            try
            {
                var advances = await _advanceService.GetAdvancesBySupplierAsync(supplierId);
                return Ok(advances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting advances for supplier {SupplierId}", supplierId);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/advances
        [HttpPost]
        public async Task<ActionResult<AdvanceDTO>> CreateAdvance([FromBody] AdvanceCreateDTO advanceDto)
        {
            try
            {
                _logger.LogInformation("Creating advance for supplier {SupplierId}", advanceDto.SupplierId);
                _logger.LogInformation("Advance data: {@AdvanceDto}", advanceDto);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state: {@ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                // Validate required fields
                if (advanceDto.SupplierId <= 0)
                {
                    return BadRequest("SupplierId is required and must be greater than 0");
                }

                if (string.IsNullOrEmpty(advanceDto.AdvanceType))
                {
                    return BadRequest("AdvanceType is required");
                }

                if (string.IsNullOrEmpty(advanceDto.Purpose))
                {
                    return BadRequest("Purpose is required");
                }

                // Calculate balance amount
                var balanceAmount = advanceDto.AdvanceAmount - advanceDto.RecoveredAmount;

                // Create the advance entity
                var advance = new Advance
                {
                    SupplierId = advanceDto.SupplierId,
                    AdvanceType = advanceDto.AdvanceType,
                    Purpose = advanceDto.Purpose,
                    AdvanceAmount = advanceDto.AdvanceAmount,
                    RecoveredAmount = advanceDto.RecoveredAmount,
                    BalanceAmount = balanceAmount >= 0 ? balanceAmount : 0,
                    IssueDate = advanceDto.IssueDate,
                    Status = advanceDto.RecoveredAmount >= advanceDto.AdvanceAmount ? "Settled" : "Active",
                    CreatedDate = DateTime.Now
                };

                var createdAdvance = await _advanceService.CreateAdvanceAsync(advance);
                var result = await _advanceService.GetAdvanceByIdAsync(createdAdvance.AdvanceId);

                _logger.LogInformation("Successfully created advance with id {AdvanceId}", createdAdvance.AdvanceId);
                return CreatedAtAction(nameof(GetAdvance), new { id = createdAdvance.AdvanceId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating advance: {@AdvanceDto}", advanceDto);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/advances/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdvance(int id, [FromBody] AdvanceCreateDTO advanceDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingAdvance = await _advanceService.GetAdvanceEntityByIdAsync(id);
                if (existingAdvance == null)
                {
                    return NotFound();
                }

                // Update the advance
                existingAdvance.SupplierId = advanceDto.SupplierId;
                existingAdvance.AdvanceType = advanceDto.AdvanceType;
                existingAdvance.Purpose = advanceDto.Purpose;
                existingAdvance.AdvanceAmount = advanceDto.AdvanceAmount;
                existingAdvance.RecoveredAmount = advanceDto.RecoveredAmount;
                existingAdvance.BalanceAmount = advanceDto.AdvanceAmount - advanceDto.RecoveredAmount;
                existingAdvance.IssueDate = advanceDto.IssueDate;
                existingAdvance.Status = advanceDto.RecoveredAmount >= advanceDto.AdvanceAmount ? "Settled" : "Active";

                var updatedAdvance = await _advanceService.UpdateAdvanceAsync(existingAdvance);
                if (updatedAdvance == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating advance with id {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/advances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdvance(int id)
        {
            try
            {
                var result = await _advanceService.DeleteAdvanceAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting advance with id {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/advances/5/deduct/{amount}
        [HttpPut("{id}/deduct/{amount}")]
        public async Task<IActionResult> DeductFromAdvance(int id, decimal amount)
        {
            try
            {
                var result = await _advanceService.DeductFromAdvanceAsync(id, amount);
                if (!result)
                {
                    return BadRequest("Invalid deduction amount or advance not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deducting from advance with id {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/advances/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTotalAdvancesCount()
        {
            try
            {
                var count = await _advanceService.GetTotalAdvancesCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total advances count");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/advances/totalOutstanding
        [HttpGet("totalOutstanding")]
        public async Task<ActionResult<decimal>> GetTotalOutstandingAmount()
        {
            try
            {
                var total = await _advanceService.GetTotalOutstandingAmountAsync();
                return Ok(total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total outstanding amount");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/advances/totalRecovered
        [HttpGet("totalRecovered")]
        public async Task<ActionResult<decimal>> GetTotalRecoveredAmount()
        {
            try
            {
                var total = await _advanceService.GetTotalRecoveredAmountAsync();
                return Ok(total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total recovered amount");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
