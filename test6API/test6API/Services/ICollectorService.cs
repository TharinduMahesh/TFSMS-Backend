using test6API.Models;

namespace test6API.Services
{
    public interface ICollectorService
    {
        // This now promises to return a list of the full CollectorAccount object.
        Task<List<CollectorAccount>> GetAllCollectorsAsync();
    }
}