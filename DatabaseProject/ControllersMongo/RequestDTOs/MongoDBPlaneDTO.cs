using Database_project.Core.MongoDB.Entities;

namespace Database_project.Core.MongoDB.RequestDTOs;

public class MongoDBPlaneDTO
{
    public string PlaneDisplayName { get; set; }

    public string AirlineId { get; set; }

    public ICollection<MongoDBFlightroute> Flightroutes { get; set; }
}