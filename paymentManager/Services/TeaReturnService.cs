using Microsoft.EntityFrameworkCore;
using paymentManager.Data;
using paymentManager.DTOs;
using paymentManager.Models;

namespace paymentManager.Services
{
    public class TeaReturnService : ITeaReturnService
    {
        private readonly ApplicationDbContext _context;

        public TeaReturnService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeaReturnDto>> GetAllAsync()
        {
            var teaReturns = await _context.TeaReturns
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return teaReturns.Select(t => new TeaReturnDto
            {
                Id = t.Id,
                Season = t.Season,
                GardenMark = t.GardenMark,
                InvoiceNumber = t.InvoiceNumber,
                ReturnDate = t.ReturnDate,
                KilosReturned = t.KilosReturned,
                Reason = t.Reason,
                CreatedAt = t.CreatedAt
            });
        }

        public async Task<TeaReturnDto?> GetByIdAsync(int id)
        {
            var teaReturn = await _context.TeaReturns.FindAsync(id);
            if (teaReturn == null) return null;

            return new TeaReturnDto
            {
                Id = teaReturn.Id,
                Season = teaReturn.Season,
                GardenMark = teaReturn.GardenMark,
                InvoiceNumber = teaReturn.InvoiceNumber,
                ReturnDate = teaReturn.ReturnDate,
                KilosReturned = teaReturn.KilosReturned,
                Reason = teaReturn.Reason,
                CreatedAt = teaReturn.CreatedAt
            };
        }

        public async Task<TeaReturnDto> CreateAsync(CreateTeaReturnDto createDto)
        {
            var teaReturn = new TeaReturn
            {
                Season = createDto.Season,
                GardenMark = createDto.GardenMark,
                InvoiceNumber = createDto.InvoiceNumber,
                ReturnDate = createDto.ReturnDate,
                KilosReturned = createDto.KilosReturned,
                Reason = createDto.Reason
            };

            _context.TeaReturns.Add(teaReturn);
            await _context.SaveChangesAsync();

            return new TeaReturnDto
            {
                Id = teaReturn.Id,
                Season = teaReturn.Season,
                GardenMark = teaReturn.GardenMark,
                InvoiceNumber = teaReturn.InvoiceNumber,
                ReturnDate = teaReturn.ReturnDate,
                KilosReturned = teaReturn.KilosReturned,
                Reason = teaReturn.Reason,
                CreatedAt = teaReturn.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var teaReturn = await _context.TeaReturns.FindAsync(id);
            if (teaReturn == null) return false;

            _context.TeaReturns.Remove(teaReturn);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
