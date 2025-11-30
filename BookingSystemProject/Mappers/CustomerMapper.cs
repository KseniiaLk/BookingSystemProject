using BookingSystemProject.DTOs;
using BookingSystemProject.Models;

namespace BookingSystemProject.Mappers;

public static class CustomerMapper
{
    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            PhoneNumber = customer.PhoneNumber
        };
    }

    public static Customer ToEntity(this CreateCustomerDto dto)
    {
        return new Customer
        {
            Name = dto.Name,
            PhoneNumber = dto.PhoneNumber
        };
    }

    public static void UpdateEntity(this Customer customer, UpdateCustomerDto dto)
    {
        customer.Name = dto.Name;
        customer.PhoneNumber = dto.PhoneNumber;
    }
}
