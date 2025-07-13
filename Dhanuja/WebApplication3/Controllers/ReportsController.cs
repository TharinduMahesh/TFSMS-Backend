//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebApplication3.Data;
//using WebApplication3.Models;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Cors;

//namespace WebApplication3.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    //[EnableCors("AllowAngularOrigins")]
//    public class ReportsController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;

//        public ReportsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Report>>> GetReports()
//        {
//            try
//            {
//                return await _context.Reports.ToListAsync();
//            }
//            catch (Exception ex)
//            {
//                // return StatusCode(500, ex.Message);
//                return StatusCode(StatusCodes.Status500InternalServerError,
//            new
//            {
//                Message = "Internal Server Error",
//                Details = ex.Message
//            });
//            }
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<Report>> GetReport(int id)
//        {
//            try
//            {
//                var report = await _context.Reports.FindAsync(id);
//                if (report == null)
//                {
//                    return NotFound();
//                }
//                return report;
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }

//        [HttpPost]
//        public async Task<ActionResult<Report>> CreateReport(Report report)
//        {
//            try
//            {
//                _context.Reports.Add(report);
//                await _context.SaveChangesAsync();
//                return CreatedAtAction(nameof(GetReport), new { id = report.Id }, report);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateReport(int id, Report report)
//        {
//            if (id != report.Id)
//            {
//                return BadRequest();
//            }

//            try
//            {
//                _context.Entry(report).State = EntityState.Modified;
//                await _context.SaveChangesAsync();
//                return NoContent();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!ReportExists(id))
//                {
//                    return NotFound();
//                }
//                throw;
//            }
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteReport(int id)
//        {
//            var report = await _context.Reports.FindAsync(id);
//            if (report == null)
//            {
//                return NotFound();
//            }

//            _context.Reports.Remove(report);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool ReportExists(int id)
//        {
//            return _context.Reports.Any(e => e.Id == id);


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
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> GetReports()
        {
            try
            {
                return await _context.Reports.ToListAsync();
            }
            catch (Exception ex)
            {
                // FIX: Ensure structured JSON error response
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while retrieving reports.",
                    Details = ex.Message, // Including ex.Message is fine for development
                    Type = ex.GetType().Name
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReport(int id)
        {
            try
            {
                var report = await _context.Reports.FindAsync(id);
                if (report == null)
                {
                    return NotFound();
                }
                return report;
            }
            catch (Exception ex)
            {
                // FIX: Ensure structured JSON error response
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while retrieving report with ID {id}.",
                    Details = ex.Message,
                    Type = ex.GetType().Name
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Report>> CreateReport(Report report)
        {
            try
            {
                _context.Reports.Add(report);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetReport), new { id = report.Id }, report);
            }
            catch (Exception ex)
            {
                // FIX: Ensure structured JSON error response
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while creating the report.",
                    Details = ex.Message,
                    Type = ex.GetType().Name
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(int id, Report report)
        {
            if (id != report.Id)
            {
                return BadRequest(new { Message = "Mismatched ID.", Details = "The ID in the URL does not match the ID in the request body." });
            }

            try
            {
                _context.Entry(report).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(id))
                {
                    return NotFound(new { Message = $"Report with ID {id} not found." });
                }
                throw; // Re-throw to be caught by ErrorHandlingMiddleware or higher level
            }
            catch (Exception ex)
            {
                // FIX: Ensure structured JSON error response
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while updating report with ID {id}.",
                    Details = ex.Message,
                    Type = ex.GetType().Name
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            try
            {
                var report = await _context.Reports.FindAsync(id);
                if (report == null)
                {
                    return NotFound(new { Message = $"Report with ID {id} not found." });
                }

                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                // FIX: Ensure structured JSON error response
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"An error occurred while deleting report with ID {id}.",
                    Details = ex.Message,
                    Type = ex.GetType().Name
                });
            }
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}