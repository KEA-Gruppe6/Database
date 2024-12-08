using Database_project.Core.MongoDB.Entities;

namespace Database_project.Core.MongoDB.DTO;

public class MongoDBAirlineDTO
{
    public string AirlineName { get; set; }

    public ICollection<MongoDBPlane> Planes { get; set; } = [];
}