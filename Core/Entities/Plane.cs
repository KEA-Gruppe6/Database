using System.ComponentModel.DataAnnotations;

namespace Database_project.Core.Entities;

public class Plane
{
    [Key]
    public long PlaneId { get; set; }
    public string PlaneDisplayName { get; set; }
    
    public long AirlineId { get; set; }
    public Airline Airline { get; set; }
    
    ICollection<Departure> Departures { get; set; }
    
}