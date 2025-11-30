namespace BookingSystemProject.Models;

public class Booking
{
    public int Id { get; set; }
    public DateTime BookingDate { get; set; }
    public TimeSpan BookingTime { get; set; }
    public int NumberOfGuests { get; set; }

    // Foreign keys
    public int TableId { get; set; }
    public int CustomerId { get; set; }

    // Navigation properties
    public Table Table { get; set; } = null!;
    public Customer Customer { get; set; } = null!;

    // Helper property to get full DateTime
    public DateTime FullDateTime => BookingDate.Date.Add(BookingTime);

    // Helper property to get end time (2 hours after start)
    public DateTime EndDateTime => FullDateTime.AddHours(2);
}
