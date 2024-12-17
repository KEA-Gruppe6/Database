using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Database_project.Core.SQL.Entities;

public class Airport
{
    [Key]
    public long AirportId { get; set; }
    public string AirportName { get; set; }
    public string AirportCity { get; set; }
    public string Municipality { get; set; }
    public string AirportAbbreviation { get; set; }
    [JsonIgnore]
    public ICollection<Maintenance> Maintenances { get; set; }

}