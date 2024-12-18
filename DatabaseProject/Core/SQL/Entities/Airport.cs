using System.ComponentModel.DataAnnotations;

namespace Database_project.Core.SQL.Entities;

public class Airport
{
    [Key]
    public long AirportId { get; set; }
    [MaxLength(255)]
    public string AirportName { get; set; }
    [MaxLength(255)]
    public string AirportCity { get; set; }
    [MaxLength(255)]
    public string Municipality { get; set; }
    [MaxLength(255)]
    public string AirportAbbreviation { get; set; }
}