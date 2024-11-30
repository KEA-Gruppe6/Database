using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Database_project.Core.Entities;

public class Plane
{
    [Key]
    public long PlaneId { get; set; }
    public string PlaneDisplayName { get; set; } = string.Empty;

    public long? AirlineId { get; set; }
    [JsonIgnore]
    public Airline? Airline { get; set; }
    [JsonIgnore]

    ICollection<Flightroute> Flightroutes { get; set; } = new List<Flightroute>();

}