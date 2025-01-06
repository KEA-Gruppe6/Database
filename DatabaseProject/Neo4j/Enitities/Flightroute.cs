using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_project.Neo4j.Entities;
public class Flightroute
{
    [Key]
    public long FlightrouteId { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public Plane Plane { get; set; }
    public Airport DepartureAirport { get; set; }
    public Airport ArrivalAirport { get; set; }
}