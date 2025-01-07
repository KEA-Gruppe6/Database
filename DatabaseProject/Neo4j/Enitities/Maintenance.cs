using System.ComponentModel.DataAnnotations;

namespace Database_project.Neo4j.Entities;

public class Maintenance
{
    [Key]
    public long MaintenanceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Airport Airport { get; set; }
    public Plane Plane { get; set; }
}