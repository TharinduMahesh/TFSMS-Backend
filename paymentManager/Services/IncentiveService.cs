using paymentManager.Data;
using paymentManager.Models;
using paymentManager.DTOs;
using Microsoft.EntityFrameworkCore;

namespace paymentManager.Services
{
    public class IncentiveService : IIncentiveService
    {
        private readonly ApplicationDbContext _context;

        public IncentiveService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IncentiveDTO>> GetAllIncentivesAsync()
        {
            var incentives = await _context.Incentives
                .Include(i => i.Supplier)
                .OrderByDescending(i => i.CreatedDate)
                .ToListAsync();

            return incentives.Select(i => new IncentiveDTO
            {
                IncentiveId = i.IncentiveId,
                SupplierId = i.SupplierId,
                SupplierName = i.Supplier?.Name ?? "Unknown",
                QualityBonus = i.QualityBonus,
                LoyaltyBonus = i.LoyaltyBonus,
                TotalAmount = i.TotalAmount,
                Month = i.Month,
                CreatedDate = i.CreatedDate
            });
        }

        public async Task<IncentiveDTO> GetIncentiveByIdAsync(int id)
        {
            var incentive = await _context.Incentives
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(i => i.IncentiveId == id);

            if (incentive == null)
                return null;

            return new IncentiveDTO
            {
                IncentiveId = incentive.IncentiveId,
                SupplierId = incentive.SupplierId,
                SupplierName = incentive.Supplier?.Name ?? "Unknown",
                QualityBonus = incentive.QualityBonus,
                LoyaltyBonus = incentive.LoyaltyBonus,
                TotalAmount = incentive.TotalAmount,
                Month = incentive.Month,
                CreatedDate = incentive.CreatedDate
            };
        }

        public async Task<Incentive> GetIncentiveEntityByIdAsync(int id)
        {
            return await _context.Incentives
                .FirstOrDefaultAsync(i => i.IncentiveId == id);
        }

        public async Task<IEnumerable<IncentiveDTO>> GetIncentivesBySupplierAsync(int supplierId)
        {
            var incentives = await _context.Incentives
                .Include(i => i.Supplier)
                .Where(i => i.SupplierId == supplierId)
                .OrderByDescending(i => i.CreatedDate)
                .ToListAsync();

            return incentives.Select(i => new IncentiveDTO
            {
                IncentiveId = i.IncentiveId,
                SupplierId = i.SupplierId,
                SupplierName = i.Supplier?.Name ?? "Unknown",
                QualityBonus = i.QualityBonus,
                LoyaltyBonus = i.LoyaltyBonus,
                TotalAmount = i.TotalAmount,
                Month = i.Month,
                CreatedDate = i.CreatedDate
            });
        }

        public async Task<IncentiveDTO> GetCurrentIncentiveForSupplierAsync(int supplierId)
        {
            string currentMonth = DateTime.Now.ToString("yyyy-MM");
            var incentive = await _context.Incentives
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(i =>
                    i.SupplierId == supplierId &&
                    i.Month == currentMonth);

            if (incentive == null)
                return null;

            return new IncentiveDTO
            {
                IncentiveId = incentive.IncentiveId,
                SupplierId = incentive.SupplierId,
                SupplierName = incentive.Supplier?.Name ?? "Unknown",
                QualityBonus = incentive.QualityBonus,
                LoyaltyBonus = incentive.LoyaltyBonus,
                TotalAmount = incentive.TotalAmount,
                Month = incentive.Month,
                CreatedDate = incentive.CreatedDate
            };
        }

        public async Task<Incentive> CreateIncentiveAsync(Incentive incentive)
        {
            try
            {
                // Ensure the supplier exists
                var supplierExists = await _context.Suppliers
                    .AnyAsync(s => s.SupplierId == incentive.SupplierId);

                if (!supplierExists)
                {
                    throw new ArgumentException($"Supplier with ID {incentive.SupplierId} does not exist.");
                }

                // Check if incentive already exists for this supplier and month
                var existingIncentive = await _context.Incentives
                    .FirstOrDefaultAsync(i => i.SupplierId == incentive.SupplierId && i.Month == incentive.Month);

                if (existingIncentive != null)
                {
                    throw new InvalidOperationException($"Incentive already exists for supplier {incentive.SupplierId} for month {incentive.Month}");
                }

                // Calculate total amount
                incentive.TotalAmount = incentive.QualityBonus + incentive.LoyaltyBonus;
                incentive.CreatedDate = DateTime.Now;

                _context.Incentives.Add(incentive);
                await _context.SaveChangesAsync();

                return incentive;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating incentive: {ex.Message}", ex);
            }
        }

        public async Task<Incentive> UpdateIncentiveAsync(Incentive incentive)
        {
            try
            {
                // Recalculate total amount
                incentive.TotalAmount = incentive.QualityBonus + incentive.LoyaltyBonus;

                _context.Entry(incentive).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return incentive;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating incentive: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteIncentiveAsync(int id)
        {
            try
            {
                var incentive = await _context.Incentives.FindAsync(id);
                if (incentive == null)
                    return false;

                _context.Incentives.Remove(incentive);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting incentive: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalIncentivesCountAsync()
        {
            return await _context.Incentives.CountAsync();
        }

        public async Task<decimal> GetTotalQualityBonusAmountAsync()
        {
            return await _context.Incentives.SumAsync(i => i.QualityBonus);
        }

        public async Task<decimal> GetTotalLoyaltyBonusAmountAsync()
        {
            return await _context.Incentives.SumAsync(i => i.LoyaltyBonus);
        }

        public async Task<decimal> GetCurrentIncentiveAmountForSupplier(int supplierId)
        {
            string currentMonth = DateTime.Now.ToString("yyyy-MM");

            var incentive = await _context.Incentives
                .Where(i => i.SupplierId == supplierId && i.Month == currentMonth)
                .FirstOrDefaultAsync();

            return incentive?.TotalAmount ?? 0;
        }

        /// <summary>
        /// Gets the latest incentive record for a supplier (most recent month)
        /// </summary>
        /// <param name="supplierId">The supplier ID</param>
        /// <returns>Latest incentive record</returns>
        public async Task<Incentive?> GetLatestIncentiveForSupplier(int supplierId)
        {
            return await _context.Incentives
                .Where(i => i.SupplierId == supplierId)
                .OrderByDescending(i => i.CreatedDate)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates incentive usage when a payment is made
        /// This could be used to track how much of the incentive has been used
        /// </summary>
        /// <param name="supplierId">The supplier ID</param>
        /// <param name="usedAmount">Amount of incentive used in payment</param>
        /// <returns>Success status</returns>
        public async Task<bool> UpdateIncentiveUsage(int supplierId, decimal usedAmount)
        {
            try
            {
                string currentMonth = DateTime.Now.ToString("yyyy-MM");

                var incentive = await _context.Incentives
                    .Where(i => i.SupplierId == supplierId && i.Month == currentMonth)
                    .FirstOrDefaultAsync();

                if (incentive != null && usedAmount > 0)
                {
                    // You could add a "UsedAmount" field to track usage
                    // For now, we'll just log that the incentive was used
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
