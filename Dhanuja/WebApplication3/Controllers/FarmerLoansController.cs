// Controllers/FarmerLoansController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FarmerLoansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FarmerLoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FarmerLoan>>> GetFarmerLoans()
        {
            return await _context.FarmerLoans.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FarmerLoan>> GetFarmerLoan(int id)
        {
            var farmerLoan = await _context.FarmerLoans.FindAsync(id);
            if (farmerLoan == null)
            {
                return NotFound();
            }
            return farmerLoan;
        }

        [HttpPost]
        public async Task<ActionResult<FarmerLoan>> CreateFarmerLoan(FarmerLoan farmerLoan)
        {
            _context.FarmerLoans.Add(farmerLoan);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFarmerLoan), new { id = farmerLoan.Id }, farmerLoan);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFarmerLoan(int id, FarmerLoan farmerLoan)
        {
            if (id != farmerLoan.Id)
            {
                return BadRequest();
            }

            _context.Entry(farmerLoan).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFarmerLoan(int id)
        {
            var farmerLoan = await _context.FarmerLoans.FindAsync(id);
            if (farmerLoan == null)
            {
                return NotFound();
            }

            _context.FarmerLoans.Remove(farmerLoan);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
