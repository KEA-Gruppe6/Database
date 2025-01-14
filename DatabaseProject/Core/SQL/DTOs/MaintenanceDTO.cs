using System.ComponentModel.DataAnnotations;
using Database_project.Core.SQL.Entities;

namespace Database_project.Core.SQL.DTOs;

public class MaintenanceDTO
{
    public long MaintenanceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Airport Airport { get; set; }
    public PlaneDTO_Airline Plane { get; set; }
}