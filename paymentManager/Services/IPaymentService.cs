using paymentManager.DTOs;
using paymentManager.Models;

namespace paymentManager.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetPaymentsAsync();
        Task<Payment> GetPaymentByIdAsync(int id);
        Task<IEnumerable<Payment>> GetPaymentsBySupplierAsync(int supplierId);
        Task<IEnumerable<Payment>> GetPaymentsByMethodAsync(string method);
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Payment> CreatePaymentAsync(Payment payment, string username);
        Task<Payment> UpdatePaymentAsync(Payment payment, string username);
        Task<bool> DeletePaymentAsync(int id, string username);
        Task<int> GetTotalPaymentsCountAsync();
        Task<decimal> GetTotalPaymentsAmountAsync();
        Task<decimal> GetTotalPaymentsByMethodAsync(string method);
        Task<object> GetPaymentSummaryAsync(DateTime? startDate, DateTime? endDate);
        Task<object> ValidatePaymentAsync(PaymentCreateRequest request);
        Task<IEnumerable<object>> GetPaymentHistoryAsync(int paymentId);
        Task<byte[]> GeneratePaymentReceiptAsync(int paymentId);
        Task<byte[]> ExportPaymentsAsync(string format, DateTime? startDate, DateTime? endDate);
    }
}
