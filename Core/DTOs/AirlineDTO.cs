namespace Database_project.Core.DTOs;

public class AirlineDTO
{
    public long AirlineId { get; set; }
    public string AirlineName { get; set; }
    public List<PlaneDTO_Airline> Planes { get; set; }
}