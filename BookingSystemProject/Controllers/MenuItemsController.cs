using BookingSystemProject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingSystemProject.Mappers;
using BookingSystemProject.Repositories;

namespace BookingSystemProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MenuItemsController : ControllerBase
{
    private readonly IMenuItemRepository _menuItemRepository;

    public MenuItemsController(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItems()
    {
        var menuItems = await _menuItemRepository.GetAllAsync();
        var result = menuItems.OrderBy(m => m.Name).Select(m => m.ToDto());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MenuItemDto>> GetMenuItem(int id)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(id);
        if (menuItem == null)
            return NotFound();

        return Ok(menuItem.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<MenuItemDto>> CreateMenuItem([FromBody] CreateMenuItemDto dto)
    {
        var menuItem = dto.ToEntity();
        await _menuItemRepository.AddAsync(menuItem);

        return CreatedAtAction(nameof(GetMenuItem), new { id = menuItem.Id }, menuItem.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] UpdateMenuItemDto dto)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(id);
        if (menuItem == null)
            return NotFound();

        menuItem.UpdateEntity(dto);
        await _menuItemRepository.UpdateAsync(menuItem);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        var menuItem = await _menuItemRepository.GetByIdAsync(id);
        if (menuItem == null)
            return NotFound();

        await _menuItemRepository.DeleteAsync(menuItem);

        return NoContent();
    }
}
