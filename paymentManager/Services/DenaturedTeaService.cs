using Microsoft.EntityFrameworkCore;
using paymentManager.Data;
using paymentManager.DTOs;
using paymentManager.Models;

namespace paymentManager.Services
{
    public class DenaturedTeaService : IDenaturedTeaService
    {
        private readonly ApplicationDbContext _context;

        public DenaturedTeaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DenaturedTeaDto>> GetAllAsync()
        {
            var denaturedTeas = await _context.DenaturedTeas
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            return denaturedTeas.Select(d => new DenaturedTeaDto
            {
                Id = d.Id,
                TeaGrade = d.TeaGrade,
                QuantityKg = d.QuantityKg,
                Reason = d.Reason,
                Date = d.Date,
                InvoiceNumber = d.InvoiceNumber,
                CreatedAt = d.CreatedAt
            });
        }

        public async Task<DenaturedTeaDto?> GetByIdAsync(int id)
        {
            var denaturedTea = await _context.DenaturedTeas.FindAsync(id);
            if (denaturedTea == null) return null;

            return new DenaturedTeaDto
            {
                Id = denaturedTea.Id,
                TeaGrade = denaturedTea.TeaGrade,
                QuantityKg = denaturedTea.QuantityKg,
                Reason = denaturedTea.Reason,
                Date = denaturedTea.Date,
                InvoiceNumber = denaturedTea.InvoiceNumber,
                CreatedAt = denaturedTea.CreatedAt
            };
        }

        public async Task<DenaturedTeaDto> CreateAsync(CreateDenaturedTeaDto createDto)
        {
            var denaturedTea = new DenaturedTea
            {
                TeaGrade = createDto.TeaGrade,
                QuantityKg = createDto.QuantityKg,
                Reason = createDto.Reason,
                Date = createDto.Date,
                InvoiceNumber = createDto.InvoiceNumber
            };

            _context.DenaturedTeas.Add(denaturedTea);
            await _context.SaveChangesAsync();

            return new DenaturedTeaDto
            {
                Id = denaturedTea.Id,
                TeaGrade = denaturedTea.TeaGrade,
                QuantityKg = denaturedTea.QuantityKg,
                Reason = denaturedTea.Reason,
                Date = denaturedTea.Date,
                InvoiceNumber = denaturedTea.InvoiceNumber,
                CreatedAt = denaturedTea.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var denaturedTea = await _context.DenaturedTeas.FindAsync(id);
            if (denaturedTea == null) return false;

            _context.DenaturedTeas.Remove(denaturedTea);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
