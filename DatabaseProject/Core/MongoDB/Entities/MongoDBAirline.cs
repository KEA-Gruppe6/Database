using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database_project.Core.MongoDB.Entities;

public class MongoDBAirline
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string AirlineId { get; set; }
    
    [BsonElement("AirlineName")]
    public string AirlineName { get; set; }
    
    [BsonElement("Planes")]
    public ICollection<MongoDBPlane> Planes { get; set; }

}