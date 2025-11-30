using BookingSystemProject.Services;
using Microsoft.EntityFrameworkCore;
using BookingSystemProject.Data;
using BookingSystemProject.Models;
using BookingSystemProject.Repositories;

namespace BookingSystemProject.Services;

public class BookingService : IBookingService
{
    private readonly RestaurantDbContext _context;
    private readonly IBookingRepository _bookingRepository;

    public BookingService(RestaurantDbContext context, IBookingRepository bookingRepository)
    {
        _context = context;
        _bookingRepository = bookingRepository;
    }

    public async Task<bool> IsTableAvailableAsync(int tableId, DateTime requestedStart, DateTime requestedEnd, int? excludeBookingId = null)
    {
        var bookings = await _bookingRepository.GetBookingsByTableAndDateAsync(tableId, requestedStart.Date);

        foreach (var booking in bookings)
        {
            if (excludeBookingId.HasValue && booking.Id == excludeBookingId.Value)
                continue;

            var bookingStart = booking.BookingDate.Date.Add(booking.BookingTime);

            // A booking blocks the table 2 hours before and 2 hours after START time
            var blockStart = bookingStart.AddHours(-2);
            var blockEnd = bookingStart.AddHours(2);

            if (requestedStart < blockEnd && requestedEnd > blockStart)
                return false;
        }

        return true;
    }

    public async Task<IEnumerable<Table>> GetAvailableTablesAsync(DateTime bookingDate, TimeSpan bookingTime, int numberOfGuests)
    {
        var requestedDateTime = bookingDate.Date.Add(bookingTime);
        var requestedEndTime = requestedDateTime.AddHours(2);

        // Get all tables with sufficient capacity
        var allTables = await _context.Tables
            .Where(t => t.Capacity >= numberOfGuests)
            .ToListAsync();

        // Get all bookings for the date
        var bookings = await _bookingRepository.GetBookingsByDateAsync(bookingDate.Date);

        // Filter out tables that have conflicting bookings
        var availableTables = allTables
            .Where(table =>
            {
                var tableBookings = bookings
                    .Where(b => b.TableId == table.Id)
                    .ToList();

                foreach (var booking in tableBookings)
                {
                    var bookingStart = booking.BookingDate.Date.Add(booking.BookingTime);

                    // A booking blocks the table 2 hours before and 2 hours after START time
                    var blockStart = bookingStart.AddHours(-2);
                    var blockEnd = bookingStart.AddHours(2);

                    if (requestedDateTime < blockEnd && requestedEndTime > blockStart)
                        return false;
                }

                return true;
            })
            .ToList();

        return availableTables;
    }
}

