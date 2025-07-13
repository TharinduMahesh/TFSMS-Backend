using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using paymentManager.Models;
using paymentManager.Services;
using paymentManager.DTOs;

namespace paymentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebtsController : ControllerBase
    {
        private readonly IDebtService _debtService;
        private readonly ILogger<DebtsController> _logger;

        public DebtsController(IDebtService debtService, ILogger<DebtsController> logger)
        {
            _debtService = debtService;
            _logger = logger;
        }

        // GET: api/debts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DebtDTO>>> GetDebts()
        {
            try
            {
                _logger.LogInformation("Getting all debts");
                var debts = await _debtService.GetAllDebtsAsync();
                return Ok(debts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all debts");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/debts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DebtDTO>> GetDebt(int id)
        {
            try
            {
                var debt = await _debtService.GetDebtByIdAsync(id);
                if (debt == null)
                {
                    return NotFound();
                }
                return Ok(debt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting debt with id {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/debts/supplier/5
        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<IEnumerable<DebtDTO>>> GetDebtsBySupplier(int supplierId)
        {
            try
            {
                var debts = await _debtService.GetDebtsBySupplierAsync(supplierId);
                return Ok(debts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting debts for supplier {SupplierId}", supplierId);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/debts
        [HttpPost]
        public async Task<ActionResult<DebtDTO>> CreateDebt([FromBody] DebtCreateDTO debtDto)
        {
            try
            {
                _logger.LogInformation("Creating debt for supplier {SupplierId}", debtDto.SupplierId);
                _logger.LogInformation("Debt data: {@DebtDto}", debtDto);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state: {@ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                // Validate required fields
                if (debtDto.SupplierId <= 0)
                {
                    return BadRequest("SupplierId is required and must be greater than 0");
                }

                if (string.IsNullOrEmpty(debtDto.DebtType))
                {
                    return BadRequest("DebtType is required");
                }

                if (string.IsNullOrEmpty(debtDto.Description))
                {
                    return BadRequest("Description is required");
                }

                // Calculate balance amount
                var balanceAmount = debtDto.TotalAmount - debtDto.DeductionsMade;

                // Create the debt entity
                var debt = new Debt
                {
                    SupplierId = debtDto.SupplierId,
                    DebtType = debtDto.DebtType,
                    Description = debtDto.Description,
                    TotalAmount = debtDto.TotalAmount,
                    DeductionsMade = debtDto.DeductionsMade,
                    BalanceAmount = balanceAmount >= 0 ? balanceAmount : 0,
                    DeductionPercentage = debtDto.DeductionPercentage,
                    IssueDate = debtDto.IssueDate,
                    Status = debtDto.DeductionsMade >= debtDto.TotalAmount ? "Settled" : "Active",
                    CreatedDate = DateTime.Now
                };

                var createdDebt = await _debtService.CreateDebtAsync(debt);
                var result = await _debtService.GetDebtByIdAsync(createdDebt.DebtId);

                _logger.LogInformation("Successfully created debt with id {DebtId}", createdDebt.DebtId);
                return CreatedAtAction(nameof(GetDebt), new { id = createdDebt.DebtId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating debt: {@DebtDto}", debtDto);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/debts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDebt(int id, [FromBody] DebtCreateDTO debtDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingDebt = await _debtService.GetDebtEntityByIdAsync(id);
                if (existingDebt == null)
                {
                    return NotFound();
                }

                // Update the debt
                existingDebt.SupplierId = debtDto.SupplierId;
                existingDebt.DebtType = debtDto.DebtType;
                existingDebt.Description = debtDto.Description;
                existingDebt.TotalAmount = debtDto.TotalAmount;
                existingDebt.DeductionsMade = debtDto.DeductionsMade;
                existingDebt.BalanceAmount = debtDto.TotalAmount - debtDto.DeductionsMade;
                existingDebt.DeductionPercentage = debtDto.DeductionPercentage;
                existingDebt.IssueDate = debtDto.IssueDate;
                existingDebt.Status = debtDto.DeductionsMade >= debtDto.TotalAmount ? "Settled" : "Active";

                var updatedDebt = await _debtService.UpdateDebtAsync(existingDebt);
                if (updatedDebt == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating debt with id {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/debts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDebt(int id)
        {
            try
            {
                var result = await _debtService.DeleteDebtAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting debt with id {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/debts/5/deduct/1000
        [HttpPut("{id}/deduct/{amount}")]
        public async Task<IActionResult> DeductFromDebt(int id, decimal amount)
        {
            try
            {
                var result = await _debtService.DeductFromDebtAsync(id, amount);
                if (!result)
                {
                    return BadRequest("Invalid deduction amount or debt not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deducting from debt with id {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/debts/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTotalDebtsCount()
        {
            try
            {
                var count = await _debtService.GetTotalDebtsCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total debts count");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/debts/totalOutstanding
        [HttpGet("totalOutstanding")]
        public async Task<ActionResult<decimal>> GetTotalOutstandingAmount()
        {
            try
            {
                var total = await _debtService.GetTotalOutstandingAmountAsync();
                return Ok(total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total outstanding amount");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/debts/totalDeductions
        [HttpGet("totalDeductions")]
        public async Task<ActionResult<decimal>> GetTotalDeductionsMade()
        {
            try
            {
                var total = await _debtService.GetTotalDeductionsMadeAsync();
                return Ok(total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total deductions made");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
