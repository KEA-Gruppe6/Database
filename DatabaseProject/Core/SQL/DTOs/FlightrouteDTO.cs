using Database_project.Core.SQL.Entities;

namespace Database_project.Core.SQL.DTOs;
public class FlightrouteDTO
{
    public long FlightrouteId { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public Airport DepartureAirport { get; set; }
    public Airport ArrivalAirport { get; set; }
    public PlaneDTO Plane { get; set; }
}