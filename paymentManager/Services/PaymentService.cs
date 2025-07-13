using paymentManager.Data;
using paymentManager.DTOs;
using paymentManager.Models;
using Microsoft.EntityFrameworkCore;

namespace paymentManager.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdvanceService _advanceService;
        private readonly IDebtService _debtService;
        private readonly IIncentiveService _incentiveService;

        public PaymentService(
            ApplicationDbContext context,
            IAdvanceService advanceService = null,
            IDebtService debtService = null,
            IIncentiveService incentiveService = null)
        {
            _context = context;
            _advanceService = advanceService;
            _debtService = debtService;
            _incentiveService = incentiveService;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.Supplier)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsBySupplierAsync(int supplierId)
        {
            return await _context.Payments
                .Include(p => p.Supplier)
                .Where(p => p.SupplierId == supplierId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByMethodAsync(string method)
        {
            return await _context.Payments
                .Include(p => p.Supplier)
                .Where(p => p.PaymentMethod == method)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Include(p => p.Supplier)
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        //public async Task<PaymentCalculationResult> CalculatePaymentAsync(PaymentCalculationRequest request)
        //{
        //    // Calculate gross amount
        //    decimal grossAmount = request.LeafWeight * request.Rate;

        //    // Calculate advance deduction
        //    decimal advanceDeduction = 0;
        //    if (request.IncludeAdvances)
        //    {
        //        if (request.AdvanceAmount > 0)
        //        {
        //            advanceDeduction = request.AdvanceAmount;
        //        }
        //        else if (_advanceService != null)
        //        {
        //            try
        //            {
        //                var advances = await _advanceService.GetAdvancesBySupplierAsync(request.SupplierId);
        //                advanceDeduction = advances.Sum(a => a.BalanceAmount);
        //            }
        //            catch
        //            {
        //                advanceDeduction = 0;
        //            }
        //        }

        //        // Apply limit if specified
        //        if (request.AdvanceDeductionLimit.HasValue && advanceDeduction > request.AdvanceDeductionLimit.Value)
        //        {
        //            advanceDeduction = request.AdvanceDeductionLimit.Value;
        //        }
        //    }

        //    // Calculate debt deduction
        //    decimal debtDeduction = 0;
        //    if (request.IncludeDebts)
        //    {
        //        if (request.DebtAmount > 0)
        //        {
        //            debtDeduction = request.DebtAmount;
        //        }
        //        else if (_debtService != null)
        //        {
        //            try
        //            {
        //                var debts = await _debtService.GetDebtsBySupplierAsync(request.SupplierId);
        //                debtDeduction = debts.Sum(d => d.BalanceAmount - d.DeductionsMade);
        //            }
        //            catch
        //            {
        //                debtDeduction = 0;
        //            }
        //        }

        //        // Apply limit if specified
        //        if (request.DebtDeductionLimit.HasValue && debtDeduction > request.DebtDeductionLimit.Value)
        //        {
        //            debtDeduction = request.DebtDeductionLimit.Value;
        //        }
        //    }

        //    // Calculate incentive addition
        //    decimal incentiveAddition = 0;
        //    if (request.IncludeIncentives)
        //    {
        //        if (request.QualityBonus > 0 || request.LoyaltyBonus > 0)
        //        {
        //            incentiveAddition = request.QualityBonus + request.LoyaltyBonus;
        //        }
        //        else if (_incentiveService != null)
        //        {
        //            try
        //            {
        //                var incentive = await _incentiveService.GetCurrentIncentiveForSupplierAsync(request.SupplierId);
        //                if (incentive != null)
        //                {
        //                    incentiveAddition = incentive.QualityBonus + incentive.LoyaltyBonus;
        //                }
        //            }
        //            catch
        //            {
        //                incentiveAddition = 0;
        //            }
        //        }
        //    }

        //    // Calculate net amount
        //    decimal netAmount = grossAmount - advanceDeduction - debtDeduction + incentiveAddition;
        //    if (netAmount < 0) netAmount = 0; // Ensure net amount is not negative

        //    return new PaymentCalculationResult
        //    {
        //        SupplierId = request.SupplierId,
        //        LeafWeight = request.LeafWeight,
        //        Rate = request.Rate,
        //        GrossAmount = grossAmount,
        //        AdvanceDeduction = advanceDeduction,
        //        DebtDeduction = debtDeduction,
        //        IncentiveAddition = incentiveAddition,
        //        NetAmount = netAmount,
        //        CalculatedAt = DateTime.Now
        //    };
        //}

        public async Task<Payment> CreatePaymentAsync(Payment payment, string username)
        {
            try
            {
                // Set created by info
                payment.CreatedBy = username;
                payment.CreatedDate = DateTime.Now;

                // Add payment without transaction for now
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                // Add payment history if PaymentHistory table exists
                try
                {
                    var history = new PaymentHistory
                    {
                        PaymentId = payment.PaymentId,
                        Action = "Created",
                        ActionBy = username,
                        ActionDate = DateTime.Now,
                        Details = $"Payment of {payment.NetAmount:C} created for supplier {payment.SupplierId}"
                    };
                    _context.PaymentHistory.Add(history);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // PaymentHistory table might not exist or have issues, log but continue
                    Console.WriteLine($"Warning: Could not create payment history: {ex.Message}");
                }

                // Return payment with supplier info
                return await GetPaymentByIdAsync(payment.PaymentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating payment: {ex.Message}");
                throw;
            }
        }

        public async Task<Payment> UpdatePaymentAsync(Payment payment, string username)
        {
            var existingPayment = await _context.Payments.FindAsync(payment.PaymentId);
            if (existingPayment == null)
                return null;

            try
            {
                // Update payment
                _context.Entry(existingPayment).CurrentValues.SetValues(payment);
                await _context.SaveChangesAsync();

                // Add payment history
                try
                {
                    var history = new PaymentHistory
                    {
                        PaymentId = payment.PaymentId,
                        Action = "Updated",
                        ActionBy = username,
                        ActionDate = DateTime.Now,
                        Details = $"Payment of {payment.NetAmount:C} updated for supplier {payment.SupplierId}"
                    };
                    _context.PaymentHistory.Add(history);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Could not create payment history: {ex.Message}");
                }

                return await GetPaymentByIdAsync(payment.PaymentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating payment: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeletePaymentAsync(int id, string username)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return false;

            try
            {
                // Add payment history before deletion
                try
                {
                    var history = new PaymentHistory
                    {
                        PaymentId = payment.PaymentId,
                        Action = "Deleted",
                        ActionBy = username,
                        ActionDate = DateTime.Now,
                        Details = $"Payment of {payment.NetAmount:C} deleted for supplier {payment.SupplierId}"
                    };
                    _context.PaymentHistory.Add(history);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Could not create payment history: {ex.Message}");
                }

                // Remove payment
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting payment: {ex.Message}");
                throw;
            }
        }

        public async Task<int> GetTotalPaymentsCountAsync()
        {
            return await _context.Payments.CountAsync();
        }

        public async Task<decimal> GetTotalPaymentsAmountAsync()
        {
            return await _context.Payments.SumAsync(p => p.NetAmount);
        }

        public async Task<decimal> GetTotalPaymentsByMethodAsync(string method)
        {
            return await _context.Payments
                .Where(p => p.PaymentMethod == method)
                .SumAsync(p => p.NetAmount);
        }

        public async Task<object> GetPaymentSummaryAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Payments.AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(p => p.PaymentDate >= startDate.Value && p.PaymentDate <= endDate.Value);
            }

            var payments = await query.ToListAsync();

            if (!payments.Any())
            {
                return new
                {
                    TotalPayments = 0,
                    TotalAmount = 0m,
                    TotalGrossAmount = 0m,
                    TotalAdvanceDeductions = 0m,
                    TotalDebtDeductions = 0m,
                    TotalIncentiveAdditions = 0m,
                    AveragePayment = 0m,
                    PaymentsByMethod = new List<object>()
                };
            }

            var paymentsByMethod = payments
                .GroupBy(p => p.PaymentMethod)
                .Select(g => new { Method = g.Key, Count = g.Count(), Total = g.Sum(p => p.NetAmount) })
                .ToList();

            return new
            {
                TotalPayments = payments.Count,
                TotalAmount = payments.Sum(p => p.NetAmount),
                TotalGrossAmount = payments.Sum(p => p.GrossAmount),
                TotalAdvanceDeductions = payments.Sum(p => p.AdvanceDeduction),
                TotalDebtDeductions = payments.Sum(p => p.DebtDeduction),
                TotalIncentiveAdditions = payments.Sum(p => p.IncentiveAddition),
                AveragePayment = payments.Average(p => p.NetAmount),
                PaymentsByMethod = paymentsByMethod
            };
        }

        public async Task<object> ValidatePaymentAsync(PaymentCreateRequest request)
        {
            var errors = new List<string>();

            // Validate supplier exists
            var supplier = await _context.Suppliers.FindAsync(request.SupplierId);
            if (supplier == null)
            {
                errors.Add("Supplier not found");
            }

            // Validate amounts
            if (request.LeafWeight <= 0)
                errors.Add("Leaf weight must be greater than 0");

            if (request.Rate <= 0)
                errors.Add("Rate must be greater than 0");

            if (request.NetAmount < 0)
                errors.Add("Net amount cannot be negative");

            // Validate payment method
            var validMethods = new[] { "Cash", "Bank Transfer", "Cheque" };
            if (!validMethods.Contains(request.PaymentMethod))
                errors.Add("Invalid payment method");

            return new
            {
                isValid = errors.Count == 0,
                errors = errors
            };
        }

        public async Task<IEnumerable<object>> GetPaymentHistoryAsync(int paymentId)
        {
            try
            {
                return await _context.PaymentHistory
                    .Where(h => h.PaymentId == paymentId)
                    .OrderByDescending(h => h.ActionDate)
                    .Select(h => new
                    {
                        h.Action,
                        h.ActionBy,
                        h.ActionDate,
                        h.Details
                    })
                    .ToListAsync();
            }
            catch
            {
                // PaymentHistory table might not exist
                return new List<object>();
            }
        }

        public async Task<byte[]> GeneratePaymentReceiptAsync(int paymentId)
        {
            var payment = await GetPaymentByIdAsync(paymentId);
            if (payment == null) return null;

            // This is a placeholder - implement actual PDF generation
            var receiptContent = $"Payment Receipt\nPayment ID: {payment.PaymentId}\nSupplier: {payment.Supplier?.Name}\nAmount: {payment.NetAmount:C}\nDate: {payment.PaymentDate:yyyy-MM-dd}";
            return System.Text.Encoding.UTF8.GetBytes(receiptContent);
        }

        public async Task<byte[]> ExportPaymentsAsync(string format, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Payments.Include(p => p.Supplier).AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(p => p.PaymentDate >= startDate.Value && p.PaymentDate <= endDate.Value);
            }

            var payments = await query.ToListAsync();

            if (format.ToLower() == "csv")
            {
                var csv = "PaymentId,SupplierId,SupplierName,LeafWeight,Rate,GrossAmount,AdvanceDeduction,DebtDeduction,IncentiveAddition,NetAmount,PaymentMethod,PaymentDate\n";
                foreach (var payment in payments)
                {
                    csv += $"{payment.PaymentId},{payment.SupplierId},{payment.Supplier?.Name},{payment.LeafWeight},{payment.Rate},{payment.GrossAmount},{payment.AdvanceDeduction},{payment.DebtDeduction},{payment.IncentiveAddition},{payment.NetAmount},{payment.PaymentMethod},{payment.PaymentDate:yyyy-MM-dd}\n";
                }
                return System.Text.Encoding.UTF8.GetBytes(csv);
            }

            return null;
        }
    }
}
