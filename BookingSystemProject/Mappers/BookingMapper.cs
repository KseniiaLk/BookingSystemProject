using BookingSystemProject.DTOs;
using BookingSystemProject.Models;

namespace BookingSystemProject.Mappers;

public static class BookingMapper
{
    public static BookingDto ToDto(this Booking booking)
    {
        return new BookingDto
        {
            Id = booking.Id,
            BookingDate = booking.BookingDate,
            BookingTime = booking.BookingTime,
            NumberOfGuests = booking.NumberOfGuests,
            TableId = booking.TableId,
            CustomerId = booking.CustomerId,
            TableNumber = booking.Table?.TableNumber.ToString() ?? string.Empty,
            CustomerName = booking.Customer?.Name ?? string.Empty
        };
    }

    public static Booking ToEntity(this CreateBookingDto dto)
    {
        return new Booking
        {
            BookingDate = dto.BookingDate.Date,
            BookingTime = dto.BookingTime,
            NumberOfGuests = dto.NumberOfGuests,
            TableId = dto.TableId,
            CustomerId = dto.CustomerId
        };
    }

    public static void UpdateEntity(this Booking booking, CreateBookingDto dto)
    {
        booking.BookingDate = dto.BookingDate.Date;
        booking.BookingTime = dto.BookingTime;
        booking.NumberOfGuests = dto.NumberOfGuests;
        booking.TableId = dto.TableId;
        booking.CustomerId = dto.CustomerId;
    }
}
