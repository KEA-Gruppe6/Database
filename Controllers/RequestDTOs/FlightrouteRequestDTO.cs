using System.ComponentModel.DataAnnotations;

namespace Database_project.Controllers.RequestDTOs;

public class FlightrouteRequestDTO
{
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public long DepartureAirportId { get; set; }
    public long ArrivalAirportId { get; set; }
    public long PlaneId { get; set; }
}