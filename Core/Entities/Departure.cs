using System.ComponentModel.DataAnnotations;

namespace Database_project.Core.Entities;

public class Departure
{
    [Key]
    public long DepartureId { get; set; }

    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }

    public long DepartureAirportId { get; set; }
    public Airport DepartureAirport { get; set; }
    
    public long ArrivalAirportId { get; set; }
    public Airport ArrivalAirport { get; set; }
    
    ICollection<Ticket> Tickets { get; set; }
    
    
}