using System.ComponentModel.DataAnnotations;

namespace Database_project.Neo4j.Entities;

public class Airline
{
    [Key]
    public long AirlineId { get; set; }
    public string AirlineName { get; set; }
    public ICollection<Plane> Planes { get; set; }
}