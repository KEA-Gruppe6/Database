using System.ComponentModel.DataAnnotations;

namespace Database_project.Core.SQL.Entities;

public class OrderDTO
{
    public long OrderId { get; set; }
    public string AirlineConfirmationNumber { get; set; }

    public ICollection<TicketDTO> Tickets { get; set; }
}