using BookingSystemProject.DTOs;
using BookingSystemProject.Models;

namespace BookingSystemProject.Mappers;

public static class TableMapper
{
    public static TableDto ToDto(this Table table)
    {
        return new TableDto
        {
            Id = table.Id,
            TableNumber = table.TableNumber,
            Capacity = table.Capacity
        };
    }

    public static Table ToEntity(this CreateTableDto dto)
    {
        return new Table
        {
            TableNumber = dto.TableNumber,
            Capacity = dto.Capacity
        };
    }

    public static void UpdateEntity(this Table table, UpdateTableDto dto)
    {
        table.TableNumber = dto.TableNumber;
        table.Capacity = dto.Capacity;
    }
}