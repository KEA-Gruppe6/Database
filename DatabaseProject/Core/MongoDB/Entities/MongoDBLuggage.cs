using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database_project.Core.MongoDB.Entities;

public class MongoDBLuggage
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string LuggageId { get; set; }

    [BsonElement("Weight")]
    public double Weight { get; set; }
    
    [BsonElement("TicketId")]
    public string TicketId { get; set; }

    [BsonElement("IsCarryOn")]
    public bool IsCarryOn { get; set; } 
}