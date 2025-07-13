using paymentManager.DTOs;
using paymentManager.Models;

namespace paymentManager.Services
{
    public interface IAdvanceService
    {
        Task<IEnumerable<AdvanceDTO>> GetAllAdvancesAsync();
        Task<AdvanceDTO> GetAdvanceByIdAsync(int id);
        Task<Advance> GetAdvanceEntityByIdAsync(int id);
        Task<IEnumerable<AdvanceDTO>> GetAdvancesBySupplierAsync(int supplierId);
        Task<Advance> CreateAdvanceAsync(Advance advance);
        Task<Advance> UpdateAdvanceAsync(Advance advance);
        Task<bool> DeleteAdvanceAsync(int id);
        Task<bool> DeductFromAdvanceAsync(int id, decimal amount);
        Task<int> GetTotalAdvancesCountAsync();
        Task<decimal> GetTotalOutstandingAmountAsync();
        Task<decimal> GetTotalRecoveredAmountAsync();
    }
}
