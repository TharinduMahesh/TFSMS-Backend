using paymentManager.Models;
using paymentManager.DTOs;

namespace paymentManager.Services
{
    public interface IIncentiveService
    {
        Task<IEnumerable<IncentiveDTO>> GetAllIncentivesAsync();
        Task<IncentiveDTO> GetIncentiveByIdAsync(int id);
        Task<Incentive> GetIncentiveEntityByIdAsync(int id);
        Task<IEnumerable<IncentiveDTO>> GetIncentivesBySupplierAsync(int supplierId);
        Task<IncentiveDTO> GetCurrentIncentiveForSupplierAsync(int supplierId);
        Task<Incentive> CreateIncentiveAsync(Incentive incentive);
        Task<Incentive> UpdateIncentiveAsync(Incentive incentive);
        Task<bool> DeleteIncentiveAsync(int id);
        Task<int> GetTotalIncentivesCountAsync();
        Task<decimal> GetTotalQualityBonusAmountAsync();
        Task<decimal> GetTotalLoyaltyBonusAmountAsync();

        // NEW PAYMENT INTEGRATION METHODS
        Task<decimal> GetCurrentIncentiveAmountForSupplier(int supplierId);
        Task<Incentive?> GetLatestIncentiveForSupplier(int supplierId);
        Task<bool> UpdateIncentiveUsage(int supplierId, decimal usedAmount);
    }
}
