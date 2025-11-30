using BookingSystemProject.Models;
using BookingSystemProject.Repositories;
using Microsoft.EntityFrameworkCore;
using BookingSystemProject.Data;


namespace BookingSystemProject.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(RestaurantDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetCustomerWithBookingsAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Bookings)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
