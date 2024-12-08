namespace Database_project.Core.SQL.DTOs;

public class PlaneDTO_Airline
{
    public long PlaneId { get; set; }
    public string PlaneDisplayName { get; set; }
    public AirlineDTO Airline { get; set; }
}