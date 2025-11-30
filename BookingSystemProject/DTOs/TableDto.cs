namespace BookingSystemProject.DTOs;

public class TableDto
{
    public int Id { get; set; }
    public int TableNumber { get; set; }
    public int Capacity { get; set; }
}

public class CreateTableDto
{
    public int TableNumber { get; set; }
    public int Capacity { get; set; }
}

public class UpdateTableDto
{
    public int TableNumber { get; set; }
    public int Capacity { get; set; }
}