using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using paymentManager.Models;
using paymentManager.Services;
using paymentManager.DTOs;

namespace paymentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPayments()
        {
            try
            {
                var payments = await _paymentService.GetPaymentsAsync();
                var paymentDTOs = payments.Select(p => MapToPaymentDTO(p)).ToList();
                return Ok(paymentDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDTO>> GetPayment(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                if (payment == null)
                {
                    return NotFound(new { message = "Payment not found" });
                }

                var paymentDTO = MapToPaymentDTO(payment);
                return Ok(paymentDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/payments/supplier/5 - Match frontend service call
        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPaymentsBySupplier(int supplierId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsBySupplierAsync(supplierId);
                var paymentDTOs = payments.Select(p => MapToPaymentDTO(p)).ToList();
                return Ok(paymentDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/payments/method/Cash - Match frontend service call
        [HttpGet("method/{method}")]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPaymentsByMethod(string method)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByMethodAsync(method);
                var paymentDTOs = payments.Select(p => MapToPaymentDTO(p)).ToList();
                return Ok(paymentDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/payments/date-range?startDate=2023-01-01&endDate=2023-12-31
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPaymentsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByDateRangeAsync(startDate, endDate);
                var paymentDTOs = payments.Select(p => MapToPaymentDTO(p)).ToList();
                return Ok(paymentDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // POST: api/payments/calculate - Match frontend service call
        //[HttpPost("calculate")]
        //public async Task<ActionResult<PaymentCalculationResult>> CalculatePayment(PaymentCalculationRequest request)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var result = await _paymentService.CalculatePaymentAsync(request);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
        //    }
        //}

        // POST: api/payments - Accept Payment object directly like frontend sends
        [HttpPost]
        public async Task<ActionResult<PaymentDTO>> CreatePayment(PaymentCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Map request to Payment entity
                var payment = MapToPaymentEntity(request);

                // Get username from claims (in a real app with auth)
                string username = User.Identity?.Name ?? "System";

                var createdPayment = await _paymentService.CreatePaymentAsync(payment, username);
                var paymentDto = MapToPaymentDTO(createdPayment);

                return CreatedAtAction(nameof(GetPayment), new { id = createdPayment.PaymentId }, paymentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // PUT: api/payments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, PaymentCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var payment = MapToPaymentEntity(request);
                payment.PaymentId = id;

                // Get username from claims (in a real app with auth)
                string username = User.Identity?.Name ?? "System";

                var updatedPayment = await _paymentService.UpdatePaymentAsync(payment, username);
                if (updatedPayment == null)
                {
                    return NotFound(new { message = "Payment not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // DELETE: api/payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                // Get username from claims (in a real app with auth)
                string username = User.Identity?.Name ?? "System";

                var result = await _paymentService.DeletePaymentAsync(id, username);
                if (!result)
                {
                    return NotFound(new { message = "Payment not found" });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/payments/count - Match frontend service call
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTotalPaymentsCount()
        {
            try
            {
                var count = await _paymentService.GetTotalPaymentsCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/payments/totalAmount - Match frontend service call
        [HttpGet("totalAmount")]
        public async Task<ActionResult<decimal>> GetTotalPaymentsAmount()
        {
            try
            {
                var total = await _paymentService.GetTotalPaymentsAmountAsync();
                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/payments/totalByMethod/Cash - Match frontend service call
        [HttpGet("totalByMethod/{method}")]
        public async Task<ActionResult<decimal>> GetTotalPaymentsByMethod(string method)
        {
            try
            {
                var total = await _paymentService.GetTotalPaymentsByMethodAsync(method);
                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/payments/summary - Match frontend service call
        [HttpGet("summary")]
        public async Task<ActionResult<object>> GetPaymentSummary(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var summary = await _paymentService.GetPaymentSummaryAsync(startDate, endDate);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // POST: api/payments/validate - Match frontend service call
        [HttpPost("validate")]
        public async Task<ActionResult<object>> ValidatePayment(PaymentCreateRequest request)
        {
            try
            {
                var validation = await _paymentService.ValidatePaymentAsync(request);
                return Ok(validation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/payments/5/history - Match frontend service call
        [HttpGet("{id}/history")]
        public async Task<ActionResult<IEnumerable<object>>> GetPaymentHistory(int id)
        {
            try
            {
                var history = await _paymentService.GetPaymentHistoryAsync(id);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/payments/5/receipt - Match frontend service call
        [HttpGet("{id}/receipt")]
        public async Task<ActionResult> GeneratePaymentReceipt(int id)
        {
            try
            {
                var receipt = await _paymentService.GeneratePaymentReceiptAsync(id);
                if (receipt == null)
                {
                    return NotFound(new { message = "Payment not found" });
                }

                return File(receipt, "application/pdf", $"payment-receipt-{id}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // GET: api/payments/export - Match frontend service call
        [HttpGet("export")]
        public async Task<ActionResult> ExportPayments(
            [FromQuery] string format = "csv",
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var exportData = await _paymentService.ExportPaymentsAsync(format, startDate, endDate);
                if (exportData == null)
                {
                    return BadRequest(new { message = "Invalid export format" });
                }

                var contentType = format.ToLower() == "excel" ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "text/csv";
                var fileName = $"payments-export.{(format.ToLower() == "excel" ? "xlsx" : "csv")}";

                return File(exportData, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        // Updated mapping methods to match your existing Supplier model structure
        private PaymentDTO MapToPaymentDTO(Payment payment)
        {
            return new PaymentDTO
            {
                PaymentId = payment.PaymentId,
                SupplierId = payment.SupplierId,
                LeafWeight = payment.LeafWeight,
                Rate = payment.Rate,
                GrossAmount = payment.GrossAmount,
                AdvanceDeduction = payment.AdvanceDeduction,
                DebtDeduction = payment.DebtDeduction,
                IncentiveAddition = payment.IncentiveAddition,
                NetAmount = payment.NetAmount,
                PaymentMethod = payment.PaymentMethod,
                PaymentDate = payment.PaymentDate,
                CreatedBy = payment.CreatedBy ?? "System",
                CreatedDate = payment.CreatedDate,
                Supplier = payment.Supplier != null ? new SupplierDTO
                {
                    SupplierId = payment.Supplier.SupplierId,
                    Name = payment.Supplier.Name ?? "",
                    Contact = payment.Supplier.Contact ?? "", // Direct mapping now
                    Area = payment.Supplier.Area ?? "", // Direct mapping now
                    JoinDate = payment.Supplier.JoinDate, // Direct mapping now
                    IsActive = payment.Supplier.IsActive
                } : null,
                Receipts = new List<PaymentReceiptDTO>()
            };
        }

        private Payment MapToPaymentEntity(PaymentCreateRequest request)
        {
            return new Payment
            {
                PaymentId = request.PaymentId,
                SupplierId = request.SupplierId,
                LeafWeight = request.LeafWeight,
                Rate = request.Rate,
                GrossAmount = request.GrossAmount,
                AdvanceDeduction = request.AdvanceDeduction,
                DebtDeduction = request.DebtDeduction,
                IncentiveAddition = request.IncentiveAddition,
                NetAmount = request.NetAmount,
                PaymentMethod = request.PaymentMethod,
                PaymentDate = request.PaymentDate,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate
            };
        }
    }
}
