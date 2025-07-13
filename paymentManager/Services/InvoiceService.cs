using Microsoft.EntityFrameworkCore;
using paymentManager.Data;
using paymentManager.DTOs;

namespace paymentManager.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllAsync()
        {
            var invoices = await _context.Invoices
                .OrderByDescending(i => i.InvoiceDate)
                .ToListAsync();

            return invoices.Select(i => new InvoiceDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                Season = i.Season,
                GardenMark = i.GardenMark,
                InvoiceDate = i.InvoiceDate,
                TotalAmount = i.TotalAmount
            });
        }

        public async Task<InvoiceDto?> GetByIdAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null) return null;

            return new InvoiceDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Season = invoice.Season,
                GardenMark = invoice.GardenMark,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount
            };
        }
    }
}
