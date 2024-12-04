using System.ComponentModel.DataAnnotations;

namespace Database_project.Controllers.RequestDTOs;

public class MaintenanceRequestDTO
{
    public long MaintenanceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public long AirportId { get; set; }
    public long PlaneId { get; set; }
}