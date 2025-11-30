using BookingSystemProject.Repositories;
using Microsoft.EntityFrameworkCore;
using BookingSystemProject.Data;
using BookingSystemProject.Models;

namespace BookingSystemProject.Repositories;

public class TableRepository : Repository<Table>, ITableRepository
{
    public TableRepository(RestaurantDbContext context) : base(context)
    {
    }

    public async Task<Table?> GetByTableNumberAsync(int tableNumber)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.TableNumber == tableNumber);
    }

    public async Task<bool> TableNumberExistsAsync(int tableNumber, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _dbSet.AnyAsync(t => t.TableNumber == tableNumber && t.Id != excludeId.Value);
        }
        return await _dbSet.AnyAsync(t => t.TableNumber == tableNumber);
    }
}
