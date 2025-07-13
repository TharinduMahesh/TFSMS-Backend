//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebApplication3.Data;
//using WebApplication3.Models;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace WebApplication3.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TeaPackingLedgerController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;

//        public TeaPackingLedgerController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<TeaPackingLedger>>> GetLedgers()
//        {
//            try
//            {
//                return await _context.TeaPackingLedgers.ToListAsync();
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }

//        [HttpGet("{saleId}")]
//        public async Task<ActionResult<TeaPackingLedger>> GetLedger(string saleId)
//        {
//            try
//            {
//                var ledger = await _context.TeaPackingLedgers.FindAsync(saleId);
//                if (ledger == null)
//                {
//                    return NotFound();
//                }
//                return ledger;
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }

//        [HttpPut("{saleId}")]
//        public async Task<IActionResult> UpdateLedger(string saleId, TeaPackingLedger ledger)
//        {
//            if (saleId != ledger.SaleId)
//            {
//                return BadRequest();
//            }

//            try
//            {
//                _context.Entry(ledger).State = EntityState.Modified;
//                await _context.SaveChangesAsync();
//                return NoContent();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!LedgerExists(saleId))
//                {
//                    return NotFound();
//                }
//                throw;
//            }
//        }

//        [HttpPost]
//        public async Task<ActionResult<TeaPackingLedger>> CreateLedger(TeaPackingLedger ledger)
//        {
//            try
//            {
//                _context.TeaPackingLedgers.Add(ledger);
//                await _context.SaveChangesAsync();
//                return CreatedAtAction(nameof(GetLedger), new { saleId = ledger.SaleId }, ledger);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }

//        [HttpDelete("{saleId}")]
//        public async Task<IActionResult> DeleteLedger(string saleId)
//        {
//            try
//            {
//                var ledger = await _context.TeaPackingLedgers.FindAsync(saleId);
//                if (ledger == null)
//                {
//                    return NotFound();
//                }

//                _context.TeaPackingLedgers.Remove(ledger);
//                await _context.SaveChangesAsync();
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }

//        private bool LedgerExists(string saleId)
//        {
//            return _context.TeaPackingLedgers.Any(e => e.SaleId == saleId);
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // ADD THIS for StatusCodes

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeaPackingLedgerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeaPackingLedgerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeaPackingLedger>>> GetLedgers()
        {
            try
            {
                return await _context.TeaPackingLedgers.ToListAsync();
            }
            catch (Exception ex)
            {
                // FIX: Return structured JSON error response
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while retrieving tea packing ledger records.",
                    Details = ex.Message, // Include ex.Message for debugging in dev
                    Type = ex.GetType().Name
                });
            }
        }

        [HttpGet("{saleId}")]
        public async Task<ActionResult<TeaPackingLedger>> GetLedger(string saleId)
        {
            try
            {
                var ledger = await _context.TeaPackingLedgers.FindAsync(saleId);
                if (ledger == null)
                {
                    return NotFound();
                }
                return ledger;
            }
            catch (Exception ex)
            {
                // FIX: Return structured JSON error response
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while retrieving ledger record with ID {saleId}.",
                    Details = ex.Message,
                    Type = ex.GetType().Name
                });
            }
        }

        [HttpPut("{saleId}")]
        public async Task<IActionResult> UpdateLedger(string saleId, TeaPackingLedger ledger)
        {
            if (saleId != ledger.SaleId)
            {
                return BadRequest(new { Message = "Mismatched ID.", Details = "The ID in the URL does not match the ID in the request body." });
            }

            try
            {
                _context.Entry(ledger).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LedgerExists(saleId))
                {
                    return NotFound(new { Message = $"Ledger record with ID {saleId} not found." });
                }
                throw; // Re-throw to be caught by ErrorHandlingMiddleware or higher level
            }
            catch (Exception ex)
            {
                // FIX: Return structured JSON error response
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while updating ledger record with ID {saleId}.",
                    Details = ex.Message,
                    Type = ex.GetType().Name
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<TeaPackingLedger>> CreateLedger(TeaPackingLedger ledger)
        {
            try
            {
                _context.TeaPackingLedgers.Add(ledger);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetLedger), new { saleId = ledger.SaleId }, ledger);
            }
            catch (Exception ex)
            {
                // FIX: Return structured JSON error response
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while creating the ledger record.",
                    Details = ex.Message,
                    Type = ex.GetType().Name
                });
            }
        }

        [HttpDelete("{saleId}")]
        public async Task<IActionResult> DeleteLedger(string saleId)
        {
            try
            {
                var ledger = await _context.TeaPackingLedgers.FindAsync(saleId);
                if (ledger == null)
                {
                    return NotFound(new { Message = $"Ledger record with ID {saleId} not found." });
                }

                _context.TeaPackingLedgers.Remove(ledger);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                // FIX: Return structured JSON error response
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while deleting ledger record with ID {saleId}.",
                    Details = ex.Message,
                    Type = ex.GetType().Name
                });
            }
        }

        private bool LedgerExists(string saleId)
        {
            return _context.TeaPackingLedgers.Any(e => e.SaleId == saleId);
        }
    }
}