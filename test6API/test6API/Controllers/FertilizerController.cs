using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Dtos;

namespace TeaFactoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FertilizerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FertilizerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/fertilizer/grower/5
        [HttpGet("grower/{growerId}")]
        public async Task<ActionResult<object>> GetFertilizerByGrower(int growerId)
        {
            var fertilizerList = await _context.Fertilizers
                .Where(f => f.GrowerId == growerId)
                .OrderByDescending(f => f.Date)
                .Select(f => new FertilizerDto
                {
                    RefNumber = f.RefNumber,
                    Date = f.Date,
                    FertilizerAmount = f.FertilizerAmount
                })
                .ToListAsync();

            var totalAmount = fertilizerList.Sum(f => f.FertilizerAmount);

            return Ok(new
            {
                totalFertilizer = totalAmount,
                records = fertilizerList
            });
        }
    }
}
