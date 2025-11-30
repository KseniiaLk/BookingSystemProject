using BookingSystemProject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingSystemProject.Data;
using BookingSystemProject.Mappers;
using BookingSystemProject.Repositories;

namespace BookingSystemProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TablesController : ControllerBase
{
    private readonly ITableRepository _tableRepository;
    private readonly RestaurantDbContext _context;

    public TablesController(ITableRepository tableRepository, RestaurantDbContext context)
    {
        _tableRepository = tableRepository;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TableDto>>> GetTables()
    {
        var tables = await _tableRepository.GetAllAsync();
        var result = tables.OrderBy(t => t.TableNumber).Select(t => t.ToDto());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TableDto>> GetTable(int id)
    {
        var table = await _tableRepository.GetByIdAsync(id);
        if (table == null)
            return NotFound();

        return Ok(table.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<TableDto>> CreateTable([FromBody] CreateTableDto dto)
    {
        // Check if table number already exists
        if (await _tableRepository.TableNumberExistsAsync(dto.TableNumber))
            return BadRequest(new { message = "Table number already exists" });

        var table = dto.ToEntity();
        await _tableRepository.AddAsync(table);

        return CreatedAtAction(nameof(GetTable), new { id = table.Id }, table.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTable(int id, [FromBody] UpdateTableDto dto)
    {
        var table = await _tableRepository.GetByIdAsync(id);
        if (table == null)
            return NotFound();

        // Check if table number already exists (excluding current table)
        if (await _tableRepository.TableNumberExistsAsync(dto.TableNumber, id))
            return BadRequest(new { message = "Table number already exists" });

        table.UpdateEntity(dto);
        await _tableRepository.UpdateAsync(table);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTable(int id)
    {
        var table = await _context.Tables
            .Include(t => t.Bookings)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (table == null)
            return NotFound();

        // Check if table has bookings
        if (table.Bookings.Any())
            return BadRequest(new { message = "Cannot delete table with existing bookings" });

        await _tableRepository.DeleteAsync(table);

        return NoContent();
    }
}