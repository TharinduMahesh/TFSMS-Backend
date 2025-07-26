using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Models;

namespace test6API.Services
{
    public class CollectorService : ICollectorService
    {
        private readonly ApplicationDbContext _context;

        public CollectorService(ApplicationDbContext context)
        {
            _context = context;
        }

        // The implementation is updated to return the full list of collectors.
        public async Task<List<CollectorAccount>> GetAllCollectorsAsync()
        {
            return await _context.CollectorCreateAccounts.ToListAsync();
        }
    }
}
