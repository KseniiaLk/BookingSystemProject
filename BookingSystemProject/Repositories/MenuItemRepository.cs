using BookingSystemProject.Models;
using BookingSystemProject.Repositories;
using BookingSystemProject.Data;

namespace BookingSystemProject.Repositories;

public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
{
    public MenuItemRepository(RestaurantDbContext context) : base(context)
    {
    }
}
