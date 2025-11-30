using BookingSystemProject.DTOs;
using BookingSystemProject.Models;

namespace BookingSystemProject.Mappers;

public static class MenuItemMapper
{
    public static MenuItemDto ToDto(this MenuItem menuItem)
    {
        return new MenuItemDto
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Price = menuItem.Price,
            Description = menuItem.Description,
            IsPopular = menuItem.IsPopular,
            BildUrl = menuItem.BildUrl
        };
    }

    public static MenuItem ToEntity(this CreateMenuItemDto dto)
    {
        return new MenuItem
        {
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Description,
            IsPopular = dto.IsPopular,
            BildUrl = dto.BildUrl
        };
    }

    public static void UpdateEntity(this MenuItem menuItem, UpdateMenuItemDto dto)
    {
        menuItem.Name = dto.Name;
        menuItem.Price = dto.Price;
        menuItem.Description = dto.Description;
        menuItem.IsPopular = dto.IsPopular;
        menuItem.BildUrl = dto.BildUrl;
    }
}

