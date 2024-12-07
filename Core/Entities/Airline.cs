using System.ComponentModel.DataAnnotations;

namespace Database_project.Core.Entities;

public class Airline
{
    [Key]
    public long AirlineId { get; set; }
    public string AirlineName { get; set; }
    public ICollection<Plane> Planes { get; set; }
}