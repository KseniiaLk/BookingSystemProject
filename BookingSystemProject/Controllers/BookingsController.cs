using BookingSystemProject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingSystemProject.Data;
using BookingSystemProject.Mappers;
using BookingSystemProject.Models;
using BookingSystemProject.Repositories;
using BookingSystemProject.Services;

namespace BookingSystemProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ITableRepository _tableRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBookingService _bookingService;

    public BookingsController(
        IBookingRepository bookingRepository,
        ITableRepository tableRepository,
        ICustomerRepository customerRepository,
        IBookingService bookingService)
    {
        _bookingRepository = bookingRepository;
        _tableRepository = tableRepository;
        _customerRepository = customerRepository;
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
    {
        var bookings = await _bookingRepository.GetBookingsWithDetailsAsync();
        var result = bookings.Select(b => b.ToDto());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetBooking(int id)
    {
        var booking = await _bookingRepository.GetBookingWithDetailsAsync(id);
        if (booking == null)
            return NotFound();

        return Ok(booking.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] CreateBookingDto dto)
    {
        // Validate table exists
        var table = await _tableRepository.GetByIdAsync(dto.TableId);
        if (table == null)
            return BadRequest(new { message = "Table not found" });

        // Validate customer exists
        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
        if (customer == null)
            return BadRequest(new { message = "Customer not found" });

        // Validate capacity
        if (dto.NumberOfGuests > table.Capacity)
            return BadRequest(new { message = "Number of guests exceeds table capacity" });

        // Check if table is available
        var requestedDateTime = dto.BookingDate.Date.Add(dto.BookingTime);
        var requestedEndTime = requestedDateTime.AddHours(2);

        var isAvailable = await _bookingService.IsTableAvailableAsync(dto.TableId, requestedDateTime, requestedEndTime);
        if (!isAvailable)
            return BadRequest(new { message = "Table is not available at the requested time" });

        var booking = dto.ToEntity();
        await _bookingRepository.AddAsync(booking);

        // Reload with navigation properties
        var bookingWithDetails = await _bookingRepository.GetBookingWithDetailsAsync(booking.Id);

        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, bookingWithDetails!.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] CreateBookingDto dto)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null)
            return NotFound();

        // Validate table exists
        var table = await _tableRepository.GetByIdAsync(dto.TableId);
        if (table == null)
            return BadRequest(new { message = "Table not found" });

        // Validate customer exists
        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
        if (customer == null)
            return BadRequest(new { message = "Customer not found" });

        // Validate capacity
        if (dto.NumberOfGuests > table.Capacity)
            return BadRequest(new { message = "Number of guests exceeds table capacity" });

        // Check if table is available (excluding current booking)
        var requestedDateTime = dto.BookingDate.Date.Add(dto.BookingTime);
        var requestedEndTime = requestedDateTime.AddHours(2);

        var isAvailable = await _bookingService.IsTableAvailableAsync(dto.TableId, requestedDateTime, requestedEndTime, id);
        if (!isAvailable)
            return BadRequest(new { message = "Table is not available at the requested time" });

        booking.UpdateEntity(dto);
        await _bookingRepository.UpdateAsync(booking);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null)
            return NotFound();

        await _bookingRepository.DeleteAsync(booking);

        return NoContent();
    }

    [HttpPost("available-tables")]
    public async Task<ActionResult<IEnumerable<TableDto>>> GetAvailableTables([FromBody] AvailableTablesRequestDto request)
    {
        var availableTables = await _bookingService.GetAvailableTablesAsync(
            request.BookingDate,
            request.BookingTime,
            request.NumberOfGuests);

        var result = availableTables
            .OrderBy(t => t.TableNumber)
            .Select(t => t.ToDto())
            .ToList();

        return Ok(result);
    }
}