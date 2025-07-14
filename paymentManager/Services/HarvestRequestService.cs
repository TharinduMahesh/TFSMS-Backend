using paymentManager.DTOs;
using paymentManager.Models;
using System;
using paymentManager.Data;


namespace paymentManager.Services;

public class HarvestRequestService
{
    private readonly ApplicationDbContext _context;

    public HarvestRequestService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HarvestRequest> SubmitRequestAsync(HarvestRequestDto dto)
    {
        var request = new HarvestRequest
        {
            Date = dto.Date,
            Time = dto.Time,
            SupperLeafWeight = dto.SupperLeafWeight,
            NormalLeafWeight = dto.NormalLeafWeight,
            TransportMethod = dto.TransportMethod,
            PaymentMethod = dto.PaymentMethod,
            Address = dto.Address,
            GrowerAccountId = dto.GrowerAccountId
        };

        _context.HarvestRequests.Add(request);
        await _context.SaveChangesAsync();

        return request;
    }
}
