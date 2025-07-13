using Microsoft.AspNetCore.Mvc;
using paymentManager.Models;
using paymentManager.Services;
using paymentManager.DTOs;

namespace paymentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        // GET: api/suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetSuppliers()
        {
            try
            {
                var suppliers = await _supplierService.GetAllSuppliersAsync();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/suppliers/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetActiveSuppliers()
        {
            try
            {
                var suppliers = await _supplierService.GetActiveSuppliersAsync();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDTO>> GetSupplier(int id)
        {
            try
            {
                var supplier = await _supplierService.GetSupplierDTOByIdAsync(id);
                if (supplier == null)
                {
                    return NotFound();
                }
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/suppliers/search?term=value
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> SearchSuppliers([FromQuery] string term)
        {
            try
            {
                var suppliers = await _supplierService.SearchSuppliersAsync(term);
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/suppliers
        [HttpPost]
        public async Task<ActionResult<SupplierDTO>> CreateSupplier(Supplier supplier)
        {
            try
            {
                var createdSupplier = await _supplierService.CreateSupplierAsync(supplier);
                var supplierDto = await _supplierService.GetSupplierDTOByIdAsync(createdSupplier.SupplierId);
                return CreatedAtAction(nameof(GetSupplier), new { id = createdSupplier.SupplierId }, supplierDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/suppliers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, Supplier supplier)
        {
            if (id != supplier.SupplierId)
            {
                return BadRequest();
            }

            try
            {
                var updatedSupplier = await _supplierService.UpdateSupplierAsync(supplier);
                if (updatedSupplier == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                var result = await _supplierService.DeleteSupplierAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}