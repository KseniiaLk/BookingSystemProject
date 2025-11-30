namespace BookingSystemProject.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    // Navigation property
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}