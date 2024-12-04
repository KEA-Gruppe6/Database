using System.ComponentModel.DataAnnotations;

namespace Database_project.Core.Entities;

public class Maintenance
{
    [Key]
    public long MaintenanceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public long AirportId { get; set; }
    public Airport Airport { get; set; }

    public long PlaneId { get; set; }
    public Plane Plane { get; set; }
}