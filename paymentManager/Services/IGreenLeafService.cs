//using paymentManager.DTOs;
//using paymentManager.Models;

//namespace paymentManager.Services
//{
//    public interface IGreenLeafService
//    {
//        Task<IEnumerable<GreenLeafDataDTO>> GetAllGreenLeafDataAsync();
//        Task<GreenLeafDataDTO> GetGreenLeafDataByIdAsync(int id);
//        Task<IEnumerable<GreenLeafDataDTO>> GetGreenLeafDataBySupplierAsync(int supplierId);
//        Task<decimal> GetLatestGreenLeafWeightAsync(int supplierId);
//        Task<decimal> GetTotalGreenLeafBySupplierAsync(int supplierId, DateTime startDate, DateTime endDate);
//        Task<object> GetGreenLeafSummaryAsync(DateTime startDate, DateTime endDate);
//        Task<GreenLeafData> CreateGreenLeafDataAsync(GreenLeafData greenLeafData);
//        Task<GreenLeafData> UpdateGreenLeafDataAsync(GreenLeafData greenLeafData);
//        Task<bool> DeleteGreenLeafDataAsync(int id);
//    }
//}