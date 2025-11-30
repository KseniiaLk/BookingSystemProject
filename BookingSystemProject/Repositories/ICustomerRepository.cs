using BookingSystemProject.Models;
using BookingSystemProject.Repositories;

namespace BookingSystemProject.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetCustomerWithBookingsAsync(int id);
}

