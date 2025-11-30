using BookingSystemProject.Models;
using BookingSystemProject.Repositories;

namespace BookingSystemProject.Repositories;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetBookingsWithDetailsAsync();
    Task<Booking?> GetBookingWithDetailsAsync(int id);
    Task<IEnumerable<Booking>> GetBookingsByDateAsync(DateTime date);
    Task<IEnumerable<Booking>> GetBookingsByTableAndDateAsync(int tableId, DateTime date);
}

