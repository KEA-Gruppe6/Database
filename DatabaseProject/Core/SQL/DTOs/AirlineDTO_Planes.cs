namespace Database_project.Core.SQL.DTOs;

public class AirlineDTO_Planes
{
    public long AirlineId { get; set; }
    public string AirlineName { get; set; }
    public List<PlaneDTO> Planes { get; set; }
}