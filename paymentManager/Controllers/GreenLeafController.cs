// Controllers/GreenLeafController.cs
// Updated to use GreenLeafData and match your Angular service expectations
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using paymentManager.Services;
using paymentManager.Models;

namespace paymentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreenLeafController : ControllerBase
    {
        private readonly GreenLeafService _greenLeafDataService;

        public GreenLeafController(GreenLeafService greenLeafDataService)
        {
            _greenLeafDataService = greenLeafDataService;
        }

        /// <summary>
        /// GET: api/GreenLeaf/supplier/{supplierId}/latest-weight
        /// Retrieves the latest green leaf weight for a specific supplier.
        /// This matches your Angular service method: getLatestGreenLeafWeight()
        /// </summary>
        /// <param name="supplierId">The ID of the supplier.</param>
        /// <returns>The latest green leaf weight as a decimal.</returns>
        [HttpGet("supplier/{supplierId}/latest-weight")]
        public async Task<ActionResult<decimal>> GetLatestGreenLeafWeight(int supplierId)
        {
            try
            {
                var weight = await _greenLeafDataService.GetLatestGreenLeafWeightBySupplierId(supplierId);
                return Ok(weight);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving green leaf weight: {ex.Message}");
            }
        }

        /// <summary>
        /// GET: api/GreenLeaf/supplier/{supplierId}
        /// Retrieves all green leaf data for a specific supplier.
        /// This matches your Angular service method: getGreenLeafDataBySupplier()
        /// </summary>
        /// <param name="supplierId">The ID of the supplier.</param>
        /// <returns>List of GreenLeafData for the supplier.</returns>
        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<List<GreenLeafData>>> GetGreenLeafDataBySupplier(int supplierId)
        {
            try
            {
                var data = await _greenLeafDataService.GetGreenLeafDataBySupplier(supplierId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving green leaf data: {ex.Message}");
            }
        }

        /// <summary>
        /// GET: api/GreenLeaf/supplier/{supplierId}/total
        /// Retrieves total green leaf weight for a supplier within a date range.
        /// This matches your Angular service method: getTotalGreenLeafBySupplier()
        /// </summary>
        /// <param name="supplierId">The ID of the supplier.</param>
        /// <param name="startDate">Start date for the range.</param>
        /// <param name="endDate">End date for the range.</param>
        /// <returns>Total weight within the date range.</returns>
        [HttpGet("supplier/{supplierId}/total")]
        public async Task<ActionResult<decimal>> GetTotalGreenLeafBySupplier(
            int supplierId,
            [FromQuery] string startDate,
            [FromQuery] string endDate)
        {
            try
            {
                if (!DateTime.TryParse(startDate, out DateTime start) ||
                    !DateTime.TryParse(endDate, out DateTime end))
                {
                    return BadRequest("Invalid date format. Please use YYYY-MM-DD format.");
                }

                var totalWeight = await _greenLeafDataService.GetTotalGreenLeafBySupplier(supplierId, start, end);
                return Ok(totalWeight);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving total green leaf weight: {ex.Message}");
            }
        }
    }
}
