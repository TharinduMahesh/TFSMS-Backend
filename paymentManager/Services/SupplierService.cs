using paymentManager.Data;
using paymentManager.DTOs;
using paymentManager.Models;
using Microsoft.EntityFrameworkCore;

namespace paymentManager.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDbContext _context;

        public SupplierService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<IEnumerable<Supplier>> GetActiveSuppliersAsync()
        {
            return await _context.Suppliers
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<IEnumerable<Supplier>> SearchSuppliersAsync(string term)
        {
            if (string.IsNullOrEmpty(term))
                return await GetActiveSuppliersAsync();

            return await _context.Suppliers
                .Where(s => s.IsActive &&
                       (s.Name.Contains(term) ||
                        s.Contact.Contains(term) ||
                        s.Area.Contains(term)))
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Supplier> CreateSupplierAsync(Supplier supplier)
        {
            supplier.JoinDate = DateTime.Now;
            supplier.IsActive = true;

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<Supplier> UpdateSupplierAsync(Supplier supplier)
        {
            _context.Entry(supplier).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
                return false;

            // Soft delete - just mark as inactive
            supplier.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SupplierDTO> GetSupplierDTOByIdAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
                return null;

            return new SupplierDTO
            {
                SupplierId = supplier.SupplierId,
                Name = supplier.Name,
                Contact = supplier.Contact,
                Area = supplier.Area,
                JoinDate = supplier.JoinDate,
                IsActive = supplier.IsActive
            };
        }

        // Additional helper methods for payment system
        public async Task<IEnumerable<SupplierDTO>> GetActiveSupplierDTOsAsync()
        {
            var suppliers = await GetActiveSuppliersAsync();
            return suppliers.Select(s => new SupplierDTO
            {
                SupplierId = s.SupplierId,
                Name = s.Name,
                Contact = s.Contact,
                Area = s.Area,
                JoinDate = s.JoinDate,
                IsActive = s.IsActive
            }).ToList();
        }

        public async Task<bool> SupplierExistsAsync(int id)
        {
            return await _context.Suppliers.AnyAsync(s => s.SupplierId == id && s.IsActive);
        }
    }
}
