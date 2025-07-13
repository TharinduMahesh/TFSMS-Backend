using paymentManager.DTOs;
using paymentManager.Models;

namespace paymentManager.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<IEnumerable<Supplier>> GetActiveSuppliersAsync();
        Task<Supplier> GetSupplierByIdAsync(int id);
        Task<IEnumerable<Supplier>> SearchSuppliersAsync(string term);
        Task<Supplier> CreateSupplierAsync(Supplier supplier);
        Task<Supplier> UpdateSupplierAsync(Supplier supplier);
        Task<bool> DeleteSupplierAsync(int id);
        Task<SupplierDTO> GetSupplierDTOByIdAsync(int id);
    }
}
