using paymentManager.DTOs;

namespace paymentManager.Services
{
    public interface IDenaturedTeaService
    {
        Task<IEnumerable<DenaturedTeaDto>> GetAllAsync();
        Task<DenaturedTeaDto?> GetByIdAsync(int id);
        Task<DenaturedTeaDto> CreateAsync(CreateDenaturedTeaDto createDto);
        Task<bool> DeleteAsync(int id);
    }
}
