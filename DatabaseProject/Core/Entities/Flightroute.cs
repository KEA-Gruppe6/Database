using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_project.Core.SQL.Entities;
public class Flightroute
{
    [Key]
    public long FlightrouteId { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }

    public long PlaneId { get; set; }
    public Plane Plane { get; set; }

    public long DepartureAirportId { get; set; }
    public Airport DepartureAirport { get; set; }

    public long ArrivalAirportId { get; set; }
    public Airport ArrivalAirport { get; set; }
}