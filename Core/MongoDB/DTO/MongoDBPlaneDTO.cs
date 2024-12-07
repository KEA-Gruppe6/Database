using Database_project.Core.MongoDB.Entities;

namespace Database_project.Core.MongoDB.DTO;

public class MongoDBPlaneDTO
{
    public string PlaneDisplayName { get; set; }
    
    public string AirlineId { get; set; }
    
    public ICollection<MongoDBDeparture> Departures { get; set; }
}