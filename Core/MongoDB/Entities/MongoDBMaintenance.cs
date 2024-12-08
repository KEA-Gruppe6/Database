using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database_project.Core.MongoDB.Entities;

public class MongoDBMaintenance
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string MaintenanceId { get; set; }

    [BsonElement("AirportId")]
    public string AirportId { get; set; }
    
    [BsonElement("StartDate")]
    public DateTime StartDate { get; set; }

    [BsonElement("EndDate")]
    public DateTime EndDate { get; set; }
    
    [BsonElement("PlaneId")]
    public string PlaneId { get; set; }
}