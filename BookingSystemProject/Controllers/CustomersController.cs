using BookingSystemProject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingSystemProject.Mappers;
using BookingSystemProject.Repositories;

namespace BookingSystemProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomersController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
    {
        var customers = await _customerRepository.GetAllAsync();
        var result = customers.OrderBy(c => c.Name).Select(c => c.ToDto());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            return NotFound();

        return Ok(customer.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CreateCustomerDto dto)
    {
        var customer = dto.ToEntity();
        await _customerRepository.AddAsync(customer);

        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDto dto)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            return NotFound();

        customer.UpdateEntity(dto);
        await _customerRepository.UpdateAsync(customer);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _customerRepository.GetCustomerWithBookingsAsync(id);
        if (customer == null)
            return NotFound();

        // Check if customer has bookings
        if (customer.Bookings.Any())
            return BadRequest(new { message = "Cannot delete customer with existing bookings" });

        await _customerRepository.DeleteAsync(customer);

        return NoContent();
    }
}