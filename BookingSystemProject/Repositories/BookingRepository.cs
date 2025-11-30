using BookingSystemProject.Models;
using BookingSystemProject.Repositories;
using Microsoft.EntityFrameworkCore;
using BookingSystemProject.Data;

namespace BookingSystemProject.Repositories;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(RestaurantDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Booking>> GetBookingsWithDetailsAsync()
    {
        return await _dbSet
            .Include(b => b.Table)
            .Include(b => b.Customer)
            .OrderByDescending(b => b.BookingDate)
            .ThenByDescending(b => b.BookingTime)
            .ToListAsync();
    }

    public async Task<Booking?> GetBookingWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(b => b.Table)
            .Include(b => b.Customer)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByDateAsync(DateTime date)
    {
        return await _dbSet
            .Where(b => b.BookingDate == date.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByTableAndDateAsync(int tableId, DateTime date)
    {
        return await _dbSet
            .Where(b => b.TableId == tableId && b.BookingDate == date.Date)
            .ToListAsync();
    }
}