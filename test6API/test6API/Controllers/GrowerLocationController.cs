using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test6API.Data;
using test6API.Dtos;
using test6API.Models;

namespace test6API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrowerLocationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GrowerLocationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/GrowerLocation
        [HttpPost]
        public async Task<ActionResult<GrowerLocationResponseDto>> SaveLocation(GrowerLocationDto locationDto)
        {
            try
            {
                // Check if location already exists for this email
                var existingLocation = await _context.GrowerLocations
                    .FirstOrDefaultAsync(l => l.GrowerEmail == locationDto.GrowerEmail);

                if (existingLocation != null)
                {
                    // Update existing location
                    existingLocation.AddressLine1 = locationDto.AddressLine1;
                    existingLocation.AddressLine2 = locationDto.AddressLine2;
                    existingLocation.City = locationDto.City;
                    existingLocation.PostalCode = locationDto.PostalCode;
                    existingLocation.Latitude = locationDto.Latitude;
                    existingLocation.Longitude = locationDto.Longitude;
                    existingLocation.UpdatedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    return Ok(new GrowerLocationResponseDto
                    {
                        LocationId = existingLocation.LocationId,
                        GrowerEmail = existingLocation.GrowerEmail,
                        AddressLine1 = existingLocation.AddressLine1,
                        AddressLine2 = existingLocation.AddressLine2,
                        City = existingLocation.City,
                        PostalCode = existingLocation.PostalCode,
                        Latitude = existingLocation.Latitude,
                        Longitude = existingLocation.Longitude,
                        CreatedAt = existingLocation.CreatedAt,
                        UpdatedAt = existingLocation.UpdatedAt
                    });
                }
                else
                {
                    // Create new location
                    var newLocation = new GrowerLocation
                    {
                        GrowerEmail = locationDto.GrowerEmail,
                        AddressLine1 = locationDto.AddressLine1,
                        AddressLine2 = locationDto.AddressLine2,
                        City = locationDto.City,
                        PostalCode = locationDto.PostalCode,
                        Latitude = locationDto.Latitude,
                        Longitude = locationDto.Longitude,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.GrowerLocations.Add(newLocation);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetLocationByEmail), 
                        new { email = locationDto.GrowerEmail }, 
                        new GrowerLocationResponseDto
                        {
                            LocationId = newLocation.LocationId,
                            GrowerEmail = newLocation.GrowerEmail,
                            AddressLine1 = newLocation.AddressLine1,
                            AddressLine2 = newLocation.AddressLine2,
                            City = newLocation.City,
                            PostalCode = newLocation.PostalCode,
                            Latitude = newLocation.Latitude,
                            Longitude = newLocation.Longitude,
                            CreatedAt = newLocation.CreatedAt,
                            UpdatedAt = newLocation.UpdatedAt
                        });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving the location", error = ex.Message });
            }
        }

        // GET: api/GrowerLocation/{email}
        [HttpGet("{email}")]
        public async Task<ActionResult<GrowerLocationResponseDto>> GetLocationByEmail(string email)
        {
            try
            {
                var location = await _context.GrowerLocations
                    .FirstOrDefaultAsync(l => l.GrowerEmail == email);

                if (location == null)
                {
                    return NotFound(new { message = "Location not found for this email" });
                }

                return Ok(new GrowerLocationResponseDto
                {
                    LocationId = location.LocationId,
                    GrowerEmail = location.GrowerEmail,
                    AddressLine1 = location.AddressLine1,
                    AddressLine2 = location.AddressLine2,
                    City = location.City,
                    PostalCode = location.PostalCode,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    CreatedAt = location.CreatedAt,
                    UpdatedAt = location.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the location", error = ex.Message });
            }
        }

        // DELETE: api/GrowerLocation/{email}
        [HttpDelete("{email}")]
        public async Task<ActionResult> DeleteLocation(string email)
        {
            try
            {
                var location = await _context.GrowerLocations
                    .FirstOrDefaultAsync(l => l.GrowerEmail == email);

                if (location == null)
                {
                    return NotFound(new { message = "Location not found for this email" });
                }

                _context.GrowerLocations.Remove(location);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Location deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the location", error = ex.Message });
            }
        }
    }
} 