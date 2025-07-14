// Controllers/GrowerSignUpController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Models;

namespace test6API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrowerCreateAccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GrowerCreateAccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(GrowerCreateAccount growerAccount)
        {
            if (ModelState.IsValid)
            {
                _context.GrowerCreateAccounts.Add(growerAccount);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Grower create account successfully" });
            }

            return BadRequest(ModelState);
        }

        // GET by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<GrowerCreateAccount>> GetGrowerAccount(int id)
        {
            var grower = await _context.GrowerCreateAccounts.FindAsync(id);

            if (grower == null)
            {
                return NotFound();
            }

            return grower;
        }
    }

}
