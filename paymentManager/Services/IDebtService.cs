using paymentManager.DTOs;
using paymentManager.Models;

namespace paymentManager.Services
{
    public interface IDebtService
    {
        Task<IEnumerable<DebtDTO>> GetAllDebtsAsync();
        Task<DebtDTO> GetDebtByIdAsync(int id);
        Task<Debt> GetDebtEntityByIdAsync(int id);
        Task<IEnumerable<DebtDTO>> GetDebtsBySupplierAsync(int supplierId);
        Task<Debt> CreateDebtAsync(Debt debt);
        Task<Debt> UpdateDebtAsync(Debt debt);
        Task<bool> DeleteDebtAsync(int id);
        Task<bool> DeductFromDebtAsync(int id, decimal amount);
        Task<int> GetTotalDebtsCountAsync();
        Task<decimal> GetTotalOutstandingAmountAsync();
        Task<decimal> GetTotalDeductionsMadeAsync();
    }
}
