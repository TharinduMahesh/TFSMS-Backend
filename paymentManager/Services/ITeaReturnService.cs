using paymentManager.DTOs;

namespace paymentManager.Services
{
    public interface ITeaReturnService
    {
        Task<IEnumerable<TeaReturnDto>> GetAllAsync();
        Task<TeaReturnDto?> GetByIdAsync(int id);
        Task<TeaReturnDto> CreateAsync(CreateTeaReturnDto createDto);
        Task<bool> DeleteAsync(int id);
    }
}
