using BookingSystemProject.DTOs;
using BookingSystemProject.Models;

namespace BookingSystemProject.Services;

public interface IBookingService
{
    Task<bool> IsTableAvailableAsync(int tableId, DateTime requestedStart, DateTime requestedEnd, int? excludeBookingId = null);
    Task<IEnumerable<Table>> GetAvailableTablesAsync(DateTime bookingDate, TimeSpan bookingTime, int numberOfGuests);
}

