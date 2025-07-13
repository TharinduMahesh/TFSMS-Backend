using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using paymentManager.Data;
using paymentManager.Models;
using paymentManager.DTOs;

namespace paymentManager.Services
{
    public class AdvanceService : IAdvanceService
    {
        private readonly ApplicationDbContext _context;

        public AdvanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AdvanceDTO>> GetAllAdvancesAsync()
        {
            var advances = await _context.Advances
                .Include(a => a.Supplier)
                .OrderByDescending(a => a.IssueDate)
                .ToListAsync();

            return advances.Select(a => new AdvanceDTO
            {
                AdvanceId = a.AdvanceId,
                SupplierId = a.SupplierId,
                SupplierName = a.Supplier?.Name ?? "Unknown",
                AdvanceAmount = a.AdvanceAmount,
                BalanceAmount = a.BalanceAmount,
                Purpose = a.Purpose,
                AdvanceType = a.AdvanceType,
                RecoveredAmount = a.RecoveredAmount,
                RecoveryPercentage = a.RecoveryPercentage,
                Status = a.Status,
                IssueDate = a.IssueDate,
                CreatedBy = a.CreatedBy,
                CreatedDate = a.CreatedDate
            });
        }

        public async Task<AdvanceDTO> GetAdvanceByIdAsync(int id)
        {
            var advance = await _context.Advances
                .Include(a => a.Supplier)
                .FirstOrDefaultAsync(a => a.AdvanceId == id);

            if (advance == null)
                return null;

            return new AdvanceDTO
            {
                AdvanceId = advance.AdvanceId,
                SupplierId = advance.SupplierId,
                SupplierName = advance.Supplier?.Name ?? "Unknown",
                AdvanceAmount = advance.AdvanceAmount,
                BalanceAmount = advance.BalanceAmount,
                Purpose = advance.Purpose,
                AdvanceType = advance.AdvanceType,
                RecoveredAmount = advance.RecoveredAmount,
                RecoveryPercentage = advance.RecoveryPercentage,
                Status = advance.Status,
                IssueDate = advance.IssueDate,
                CreatedBy = advance.CreatedBy,
                CreatedDate = advance.CreatedDate
            };
        }

        public async Task<Advance> GetAdvanceEntityByIdAsync(int id)
        {
            return await _context.Advances
                .FirstOrDefaultAsync(a => a.AdvanceId == id);
        }

        public async Task<IEnumerable<AdvanceDTO>> GetAdvancesBySupplierAsync(int supplierId)
        {
            var advances = await _context.Advances
                .Include(a => a.Supplier)
                .Where(a => a.SupplierId == supplierId)
                .OrderByDescending(a => a.IssueDate)
                .ToListAsync();

            return advances.Select(a => new AdvanceDTO
            {
                AdvanceId = a.AdvanceId,
                SupplierId = a.SupplierId,
                SupplierName = a.Supplier?.Name ?? "Unknown",
                AdvanceAmount = a.AdvanceAmount,
                BalanceAmount = a.BalanceAmount,
                Purpose = a.Purpose,
                AdvanceType = a.AdvanceType,
                RecoveredAmount = a.RecoveredAmount,
                RecoveryPercentage = a.RecoveryPercentage,
                Status = a.Status,
                IssueDate = a.IssueDate,
                CreatedBy = a.CreatedBy,
                CreatedDate = a.CreatedDate
            });
        }

        public async Task<Advance> CreateAdvanceAsync(Advance advance)
        {
            try
            {
                // Ensure the supplier exists
                var supplierExists = await _context.Suppliers
                    .AnyAsync(s => s.SupplierId == advance.SupplierId);

                if (!supplierExists)
                {
                    throw new ArgumentException($"Supplier with ID {advance.SupplierId} does not exist.");
                }

                // Set the created date
                advance.CreatedDate = DateTime.Now;

                // Ensure balance amount is calculated correctly
                advance.BalanceAmount = advance.AdvanceAmount - advance.RecoveredAmount;

                // Set status based on balance
                advance.Status = advance.BalanceAmount <= 0 ? "Settled" : "Active";

                _context.Advances.Add(advance);
                await _context.SaveChangesAsync();

                return advance;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating advance: {ex.Message}", ex);
            }
        }

        public async Task<Advance> UpdateAdvanceAsync(Advance advance)
        {
            try
            {
                // Recalculate balance amount
                advance.BalanceAmount = advance.AdvanceAmount - advance.RecoveredAmount;

                // Update status based on balance
                advance.Status = advance.BalanceAmount <= 0 ? "Settled" : "Active";

                _context.Entry(advance).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return advance;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating advance: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAdvanceAsync(int id)
        {
            try
            {
                var advance = await _context.Advances.FindAsync(id);
                if (advance == null)
                    return false;

                _context.Advances.Remove(advance);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting advance: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeductFromAdvanceAsync(int id, decimal amount)
        {
            try
            {
                var advance = await _context.Advances.FindAsync(id);
                if (advance == null || amount <= 0)
                    return false;

                advance.RecoveredAmount += amount;
                advance.BalanceAmount = advance.AdvanceAmount - advance.RecoveredAmount;

                // Update status if fully recovered
                if (advance.BalanceAmount <= 0)
                {
                    advance.Status = "Settled";
                    advance.BalanceAmount = 0; // Ensure it doesn't go negative
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deducting from advance: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalAdvancesCountAsync()
        {
            return await _context.Advances.CountAsync();
        }

        public async Task<decimal> GetTotalOutstandingAmountAsync()
        {
            return await _context.Advances
                .Where(a => a.Status == "Active")
                .SumAsync(a => a.BalanceAmount);
        }

        public async Task<decimal> GetTotalRecoveredAmountAsync()
        {
            return await _context.Advances.SumAsync(a => a.RecoveredAmount);
        }
    }
}
