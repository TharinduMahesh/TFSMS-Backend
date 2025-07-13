// Services/GreenLeafDataService.cs
// Updated to use GreenLeafData instead of GreenLeaf
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using paymentManager.Data;
using paymentManager.Models;

namespace paymentManager.Services
{
    public class GreenLeafService
    {
        private readonly ApplicationDbContext _context;

        public GreenLeafService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves the latest green leaf weight for a given supplier.
        /// "Latest" is determined by the most recent CreatedDate.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier.</param>
        /// <returns>The latest green leaf weight, or 0 if no data is found.</returns>
        public async Task<decimal> GetLatestGreenLeafWeightBySupplierId(int supplierId)
        {
            var latestGreenLeafWeight = await _context.GreenLeafData
                .Where(gl => gl.SupplierId == supplierId)
                .OrderByDescending(gl => gl.CreatedDate) // Order by date to get the latest
                .Select(gl => gl.Weight)
                .FirstOrDefaultAsync();

            return latestGreenLeafWeight; // Returns 0 if no entry is found for the supplier
        }

        /// <summary>
        /// Retrieves all green leaf data for a specific supplier.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier.</param>
        /// <returns>List of GreenLeafData for the supplier.</returns>
        public async Task<List<GreenLeafData>> GetGreenLeafDataBySupplier(int supplierId)
        {
            return await _context.GreenLeafData
                .Where(gl => gl.SupplierId == supplierId)
                .OrderByDescending(gl => gl.CreatedDate)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves total green leaf weight for a supplier within a date range.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier.</param>
        /// <param name="startDate">Start date for the range.</param>
        /// <param name="endDate">End date for the range.</param>
        /// <returns>Total weight within the date range.</returns>
        public async Task<decimal> GetTotalGreenLeafBySupplier(int supplierId, DateTime startDate, DateTime endDate)
        {
            var totalWeight = await _context.GreenLeafData
                .Where(gl => gl.SupplierId == supplierId &&
                           gl.CreatedDate >= startDate &&
                           gl.CreatedDate <= endDate)
                .SumAsync(gl => gl.Weight);

            return totalWeight;
        }
    }
}
