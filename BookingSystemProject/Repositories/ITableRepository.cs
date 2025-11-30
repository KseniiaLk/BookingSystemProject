using BookingSystemProject.Repositories;
using BookingSystemProject.Models;

namespace BookingSystemProject.Repositories;

public interface ITableRepository : IRepository<Table>
{
    Task<Table?> GetByTableNumberAsync(int tableNumber);
    Task<bool> TableNumberExistsAsync(int tableNumber, int? excludeId = null);
}

