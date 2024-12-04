namespace Database_project.Core.DTOs;

public class PlaneDTO
{
    public long PlaneId { get; set; }
    public string PlaneDisplayName { get; set; }
    public AirlineDTO_Plane Airline { get; set; }
}