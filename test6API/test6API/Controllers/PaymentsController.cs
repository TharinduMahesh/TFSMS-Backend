using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Dtos;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PaymentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<PaymentResponseDto>> GetPaymentsByEmail(string email)
    {
        var payments = await _context.Payments
            .Where(p => p.GrowerEmail == email)
            .ToListAsync();

        if (payments == null || !payments.Any())
            return NotFound("No payments found for the given email.");

        var total = payments.Sum(p => p.Amount);

        var response = new PaymentResponseDto
        {
            TotalAmount = total,
            Payments = payments.Select(p => new PaymentItemDto
            {
                RefNumber = p.RefNumber,
                Amount = p.Amount,
                PaymentTime = p.PaymentTime,
                PaymentMethod = p.PaymentMethod
            }).ToList()
        };

        return Ok(response);
    }
}
