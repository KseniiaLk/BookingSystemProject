namespace BookingSystemProject.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public DateTime BookingDate { get; set; }
    public TimeSpan BookingTime { get; set; }
    public int NumberOfGuests { get; set; }
    public int TableId { get; set; }
    public int CustomerId { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
}

public class CreateBookingDto
{
    public DateTime BookingDate { get; set; }
    public TimeSpan BookingTime { get; set; }
    public int NumberOfGuests { get; set; }
    public int TableId { get; set; }
    public int CustomerId { get; set; }
}

public class AvailableTablesRequestDto
{
    public DateTime BookingDate { get; set; }
    public TimeSpan BookingTime { get; set; }
    public int NumberOfGuests { get; set; }
}