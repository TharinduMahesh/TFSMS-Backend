using paymentManager.Data;
using paymentManager.Models;
using paymentManager.DTOs;
using Microsoft.EntityFrameworkCore;

namespace paymentManager.Services
{
    public class DebtService : IDebtService
    {
        private readonly ApplicationDbContext _context;

        public DebtService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DebtDTO>> GetAllDebtsAsync()
        {
            var debts = await _context.Debts
                .Include(d => d.Supplier)
                .OrderByDescending(d => d.IssueDate)
                .ToListAsync();

            return debts.Select(d => new DebtDTO
            {
                DebtId = d.DebtId,
                SupplierId = d.SupplierId,
                SupplierName = d.Supplier?.Name ?? "Unknown",
                BalanceAmount = d.BalanceAmount,
                DeductionsMade = d.DeductionsMade,
                Description = d.Description,
                DebtType = d.DebtType,
                TotalAmount = d.TotalAmount,
                DeductionPercentage = d.DeductionPercentage,
                Status = d.Status,
                IssueDate = d.IssueDate,
                CreatedBy = d.CreatedBy,
                CreatedDate = d.CreatedDate
            });
        }

        public async Task<DebtDTO> GetDebtByIdAsync(int id)
        {
            var debt = await _context.Debts
                .Include(d => d.Supplier)
                .FirstOrDefaultAsync(d => d.DebtId == id);

            if (debt == null)
                return null;

            return new DebtDTO
            {
                DebtId = debt.DebtId,
                SupplierId = debt.SupplierId,
                SupplierName = debt.Supplier?.Name ?? "Unknown",
                BalanceAmount = debt.BalanceAmount,
                DeductionsMade = debt.DeductionsMade,
                Description = debt.Description,
                DebtType = debt.DebtType,
                TotalAmount = debt.TotalAmount,
                DeductionPercentage = debt.DeductionPercentage,
                Status = debt.Status,
                IssueDate = debt.IssueDate,
                CreatedBy = debt.CreatedBy,
                CreatedDate = debt.CreatedDate
            };
        }

        public async Task<Debt> GetDebtEntityByIdAsync(int id)
        {
            return await _context.Debts
                .FirstOrDefaultAsync(d => d.DebtId == id);
        }

        public async Task<IEnumerable<DebtDTO>> GetDebtsBySupplierAsync(int supplierId)
        {
            var debts = await _context.Debts
                .Include(d => d.Supplier)
                .Where(d => d.SupplierId == supplierId)
                .OrderByDescending(d => d.IssueDate)
                .ToListAsync();

            return debts.Select(d => new DebtDTO
            {
                DebtId = d.DebtId,
                SupplierId = d.SupplierId,
                SupplierName = d.Supplier?.Name ?? "Unknown",
                BalanceAmount = d.BalanceAmount,
                DeductionsMade = d.DeductionsMade,
                Description = d.Description,
                DebtType = d.DebtType,
                TotalAmount = d.TotalAmount,
                DeductionPercentage = d.DeductionPercentage,
                Status = d.Status,
                IssueDate = d.IssueDate,
                CreatedBy = d.CreatedBy,
                CreatedDate = d.CreatedDate
            });
        }

        public async Task<Debt> CreateDebtAsync(Debt debt)
        {
            try
            {
                // Ensure the supplier exists
                var supplierExists = await _context.Suppliers
                    .AnyAsync(s => s.SupplierId == debt.SupplierId);

                if (!supplierExists)
                {
                    throw new ArgumentException($"Supplier with ID {debt.SupplierId} does not exist.");
                }

                // Set the created date
                debt.CreatedDate = DateTime.Now;

                // Ensure balance amount is calculated correctly
                debt.BalanceAmount = debt.TotalAmount - debt.DeductionsMade;

                // Set status based on balance
                debt.Status = debt.BalanceAmount <= 0 ? "Settled" : "Active";

                _context.Debts.Add(debt);
                await _context.SaveChangesAsync();

                return debt;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating debt: {ex.Message}", ex);
            }
        }

        public async Task<Debt> UpdateDebtAsync(Debt debt)
        {
            try
            {
                // Recalculate balance amount
                debt.BalanceAmount = debt.TotalAmount - debt.DeductionsMade;

                // Update status based on balance
                debt.Status = debt.BalanceAmount <= 0 ? "Settled" : "Active";

                _context.Entry(debt).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return debt;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating debt: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteDebtAsync(int id)
        {
            try
            {
                var debt = await _context.Debts.FindAsync(id);
                if (debt == null)
                    return false;

                _context.Debts.Remove(debt);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting debt: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeductFromDebtAsync(int id, decimal amount)
        {
            try
            {
                var debt = await _context.Debts.FindAsync(id);
                if (debt == null || amount <= 0)
                    return false;

                debt.DeductionsMade += amount;
                debt.BalanceAmount = debt.TotalAmount - debt.DeductionsMade;

                // Update status if fully paid
                if (debt.BalanceAmount <= 0)
                {
                    debt.Status = "Settled";
                    debt.BalanceAmount = 0; // Ensure it doesn't go negative
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deducting from debt: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalDebtsCountAsync()
        {
            return await _context.Debts.CountAsync();
        }

        public async Task<decimal> GetTotalOutstandingAmountAsync()
        {
            return await _context.Debts
                .Where(d => d.Status == "Active")
                .SumAsync(d => d.BalanceAmount);
        }

        public async Task<decimal> GetTotalDeductionsMadeAsync()
        {
            return await _context.Debts.SumAsync(d => d.DeductionsMade);
        }
    }
}
