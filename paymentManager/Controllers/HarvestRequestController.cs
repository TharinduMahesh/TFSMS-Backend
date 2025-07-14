using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using paymentManager.DTOs;
using paymentManager.Data;
using paymentManager.Models;
using paymentManager.Services;


namespace YourProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HarvestRequestController : ControllerBase
{
    private readonly HarvestRequestService _service;
    private readonly ApplicationDbContext _context;

    public HarvestRequestController(HarvestRequestService service, ApplicationDbContext context)
    {
        _service = service;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] HarvestRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _service.SubmitRequestAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { id = created.Id });
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _context.HarvestRequests
            .Where(r => r.Status == "Pending")
            .OrderBy(r => r.Date)
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("accepted")]
    public async Task<IActionResult> GetAccepted()
    {
        var result = await _context.HarvestRequests
            .Where(r => r.Status == "Accepted")
            .OrderBy(r => r.Date)
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _context.HarvestRequests.FindAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
    {
        var item = await _context.HarvestRequests.FindAsync(id);
        if (item == null) return NotFound();

        item.Status = dto.Status;
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPatch("{id}/weights")]
    public async Task<IActionResult> UpdateWeights(int id, [FromBody] UpdateWeightsDto dto)
    {
        var item = await _context.HarvestRequests.FindAsync(id);
        if (item == null) return NotFound();

        item.SupperLeafWeight = dto.SupperLeafWeight;
        item.NormalLeafWeight = dto.NormalLeafWeight;
        item.Status = dto.Status;

        await _context.SaveChangesAsync();
        return Ok();
    }
}
