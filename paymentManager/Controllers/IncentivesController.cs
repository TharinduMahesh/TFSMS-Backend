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
    public class IncentivesController : ControllerBase
    {
        private readonly IIncentiveService _incentiveService;
        private readonly ILogger<IncentivesController> _logger;

        public IncentivesController(IIncentiveService incentiveService, ILogger<IncentivesController> logger)
        {
            _incentiveService = incentiveService;
            _logger = logger;
        }

        // GET: api/incentives
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncentiveDTO>>> GetIncentives()
        {
            try
            {
                _logger.LogInformation("Getting all incentives");
                var incentives = await _incentiveService.GetAllIncentivesAsync();
                return Ok(incentives);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all incentives");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/incentives/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IncentiveDTO>> GetIncentive(int id)
        {
            try
            {
                var incentive = await _incentiveService.GetIncentiveByIdAsync(id);
                if (incentive == null)
                {
                    return NotFound();
                }
                return Ok(incentive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting incentive with id {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/incentives/supplier/5
        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<IEnumerable<IncentiveDTO>>> GetIncentivesBySupplier(int supplierId)
        {
            try
            {
                var incentives = await _incentiveService.GetIncentivesBySupplierAsync(supplierId);
                return Ok(incentives);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting incentives for supplier {SupplierId}", supplierId);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/incentives/supplier/5/current
        [HttpGet("supplier/{supplierId}/current")]
        public async Task<ActionResult<IncentiveDTO>> GetCurrentIncentiveForSupplier(int supplierId)
        {
            try
            {
                var incentive = await _incentiveService.GetCurrentIncentiveForSupplierAsync(supplierId);
                if (incentive == null)
                {
                    return NotFound();
                }
                return Ok(incentive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current incentive for supplier {SupplierId}", supplierId);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // ===== NEW PAYMENT INTEGRATION ENDPOINTS =====

        /// <summary>
        /// GET: api/incentives/supplier/{supplierId}/current-amount
        /// Gets the current month's total incentive amount for a supplier
        /// This is used by the payment form to auto-fill incentive data
        /// </summary>
        [HttpGet("supplier/{supplierId}/current-amount")]
        public async Task<ActionResult<decimal>> GetCurrentIncentiveAmount(int supplierId)
        {
            try
            {
                _logger.LogInformation("Getting current incentive amount for supplier {SupplierId}", supplierId);
                var amount = await _incentiveService.GetCurrentIncentiveAmountForSupplier(supplierId);
                return Ok(amount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current incentive amount for supplier {SupplierId}", supplierId);
                return StatusCode(500, $"Error retrieving current incentive amount: {ex.Message}");
            }
        }

        /// <summary>
        /// GET: api/incentives/supplier/{supplierId}/latest
        /// Gets the latest incentive record for a supplier (most recent month)
        /// This provides detailed incentive information for payment processing
        /// </summary>
        [HttpGet("supplier/{supplierId}/latest")]
        public async Task<ActionResult> GetLatestIncentive(int supplierId)
        {
            try
            {
                _logger.LogInformation("Getting latest incentive for supplier {SupplierId}", supplierId);
                var incentive = await _incentiveService.GetLatestIncentiveForSupplier(supplierId);

                if (incentive == null)
                {
                    return Ok(new { TotalAmount = 0, Month = "", QualityBonus = 0, LoyaltyBonus = 0 });
                }

                return Ok(new
                {
                    incentive.TotalAmount,
                    incentive.Month,
                    incentive.QualityBonus,
                    incentive.LoyaltyBonus,
                    incentive.IncentiveId,
                    incentive.SupplierId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest incentive for supplier {SupplierId}", supplierId);
                return StatusCode(500, $"Error retrieving latest incentive: {ex.Message}");
            }
        }

        /// <summary>
        /// POST: api/incentives/supplier/{supplierId}/update-usage
        /// Updates incentive usage when a payment is made
        /// This tracks how much of the incentive has been used in payments
        /// </summary>
        [HttpPost("supplier/{supplierId}/update-usage")]
        public async Task<ActionResult<bool>> UpdateIncentiveUsage(int supplierId, [FromBody] IncentiveUsageRequest request)
        {
            try
            {
                _logger.LogInformation("Updating incentive usage for supplier {SupplierId}, amount: {UsedAmount}",
                    supplierId, request.UsedAmount);

                var success = await _incentiveService.UpdateIncentiveUsage(supplierId, request.UsedAmount);
                return Ok(success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating incentive usage for supplier {SupplierId}", supplierId);
                return StatusCode(500, $"Error updating incentive usage: {ex.Message}");
            }
        }

        // ===== END NEW PAYMENT INTEGRATION ENDPOINTS =====

        // POST: api/incentives
        [HttpPost]
        public async Task<ActionResult<IncentiveDTO>> CreateIncentive([FromBody] IncentiveCreateDTO incentiveDto)
        {
            try
            {
                _logger.LogInformation("Creating incentive for supplier {SupplierId}", incentiveDto.SupplierId);
                _logger.LogInformation("Incentive data: {@IncentiveDto}", incentiveDto);

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state: {@ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                // Validate required fields
                if (incentiveDto.SupplierId <= 0)
                {
                    return BadRequest("SupplierId is required and must be greater than 0");
                }

                if (string.IsNullOrEmpty(incentiveDto.Month))
                {
                    return BadRequest("Month is required");
                }

                // Create the incentive entity
                var incentive = new Incentive
                {
                    SupplierId = incentiveDto.SupplierId,
                    QualityBonus = incentiveDto.QualityBonus,
                    LoyaltyBonus = incentiveDto.LoyaltyBonus,
                    TotalAmount = incentiveDto.QualityBonus + incentiveDto.LoyaltyBonus,
                    Month = incentiveDto.Month,
                    CreatedDate = DateTime.Now
                };

                var createdIncentive = await _incentiveService.CreateIncentiveAsync(incentive);
                var result = await _incentiveService.GetIncentiveByIdAsync(createdIncentive.IncentiveId);

                _logger.LogInformation("Successfully created incentive with id {IncentiveId}", createdIncentive.IncentiveId);
                return CreatedAtAction(nameof(GetIncentive), new { id = createdIncentive.IncentiveId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating incentive: {@IncentiveDto}", incentiveDto);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/incentives/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncentive(int id, [FromBody] IncentiveCreateDTO incentiveDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingIncentive = await _incentiveService.GetIncentiveEntityByIdAsync(id);
                if (existingIncentive == null)
                {
                    return NotFound();
                }

                // Update the incentive
                existingIncentive.SupplierId = incentiveDto.SupplierId;
                existingIncentive.QualityBonus = incentiveDto.QualityBonus;
                existingIncentive.LoyaltyBonus = incentiveDto.LoyaltyBonus;
                existingIncentive.TotalAmount = incentiveDto.QualityBonus + incentiveDto.LoyaltyBonus;
                existingIncentive.Month = incentiveDto.Month;

                var updatedIncentive = await _incentiveService.UpdateIncentiveAsync(existingIncentive);
                if (updatedIncentive == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating incentive with id {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/incentives/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncentive(int id)
        {
            try
            {
                var result = await _incentiveService.DeleteIncentiveAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting incentive with id {Id}", id);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/incentives/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTotalIncentivesCount()
        {
            try
            {
                var count = await _incentiveService.GetTotalIncentivesCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total incentives count");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/incentives/totalQualityBonus
        [HttpGet("totalQualityBonus")]
        public async Task<ActionResult<decimal>> GetTotalQualityBonusAmount()
        {
            try
            {
                var total = await _incentiveService.GetTotalQualityBonusAmountAsync();
                return Ok(total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total quality bonus amount");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/incentives/totalLoyaltyBonus
        [HttpGet("totalLoyaltyBonus")]
        public async Task<ActionResult<decimal>> GetTotalLoyaltyBonusAmount()
        {
            try
            {
                var total = await _incentiveService.GetTotalLoyaltyBonusAmountAsync();
                return Ok(total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total loyalty bonus amount");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    // DTO for incentive usage requests
    public class IncentiveUsageRequest
    {
        public decimal UsedAmount { get; set; }
    }
}
