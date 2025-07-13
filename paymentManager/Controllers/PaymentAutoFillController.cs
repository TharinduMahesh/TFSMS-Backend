//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading.Tasks;
//using paymentManager.Services;
//using paymentManager.DTOs;
//using System.Linq;

//namespace paymentManager.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PaymentAutoFillController : ControllerBase
//    {
//        private readonly IGreenLeafService _greenLeafService;
//        private readonly IAdvanceService _advanceService;
//        private readonly IDebtService _debtService;
//        private readonly IIncentiveService _incentiveService;
//        private readonly ISupplierService _supplierService;
//        private readonly ILogger<PaymentAutoFillController> _logger;

//        public PaymentAutoFillController(
//            IGreenLeafService greenLeafService,
//            IAdvanceService advanceService,
//            IDebtService debtService,
//            IIncentiveService incentiveService,
//            ISupplierService supplierService,
//            ILogger<PaymentAutoFillController> logger)
//        {
//            _greenLeafService = greenLeafService;
//            _advanceService = advanceService;
//            _debtService = debtService;
//            _incentiveService = incentiveService;
//            _supplierService = supplierService;
//            _logger = logger;
//        }

//        // GET: api/paymentautofill/supplier/5
//        [HttpGet("supplier/{supplierId}")]
//        public async Task<ActionResult<PaymentAutoFillDataDTO>> GetPaymentAutoFillData(int supplierId)
//        {
//            try
//            {
//                _logger.LogInformation("Getting auto-fill data for supplier {SupplierId}", supplierId);

//                // Get supplier info
//                var supplier = await _supplierService.GetSupplierByIdAsync(supplierId);
//                if (supplier == null)
//                {
//                    return NotFound(new { message = "Supplier not found" });
//                }

//                // Get green leaf weight
//                var leafWeight = await _greenLeafService.GetLatestGreenLeafWeightAsync(supplierId);

//                // Get active advances
//                var advances = await _advanceService.GetAdvancesBySupplierAsync(supplierId);
//                var activeAdvances = advances.Where(a => a.Status == "Active").ToList();

//                // Calculate total advance deduction (30% of balance by default)
//                var totalAdvanceDeduction = activeAdvances.Sum(a =>
//                    a.BalanceAmount * (a.RecoveryPercentage / 100));

//                // Get active debts
//                var debts = await _debtService.GetDebtsBySupplierAsync(supplierId);
//                var activeDebts = debts.Where(d => d.Status == "Active").ToList();

//                // Calculate total debt deduction (based on deduction percentage)
//                var totalDebtDeduction = activeDebts.Sum(d =>
//                    d.BalanceAmount * (d.DeductionPercentage / 100));

//                // Get current month incentive
//                var incentive = await _incentiveService.GetCurrentIncentiveForSupplierAsync(supplierId);
//                var incentiveAddition = incentive?.TotalAmount ?? 0;

//                var autoFillData = new PaymentAutoFillDataDTO
//                {
//                    SupplierId = supplierId,
//                    SupplierName = supplier.Name,
//                    LeafWeight = leafWeight,
//                    AdvanceDeduction = Math.Round(totalAdvanceDeduction, 2),
//                    DebtDeduction = Math.Round(totalDebtDeduction, 2),
//                    IncentiveAddition = Math.Round(incentiveAddition, 2),
//                    HasActiveAdvances = activeAdvances.Any(),
//                    HasActiveDebts = activeDebts.Any(),
//                    HasCurrentIncentives = incentive != null,
//                    ActiveAdvancesCount = activeAdvances.Count,
//                    ActiveDebtsCount = activeDebts.Count
//                };

//                _logger.LogInformation("Auto-fill data retrieved for supplier {SupplierId}: {@AutoFillData}",
//                    supplierId, autoFillData);

//                return Ok(autoFillData);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting auto-fill data for supplier {SupplierId}", supplierId);
//                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
//            }
//        }
//    }
//}
