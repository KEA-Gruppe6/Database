using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database_project.Core.MongoDB.Entities;

public class MongoDBOrder
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] 
    public string OrderId { get; set; }

    [BsonElement("AirlineConfirmationNumber")]
    public string AirlineConfirmationNumber { get; set; }

    [BsonElement("TicketIds")]
    [BsonRepresentation(BsonType.ObjectId)] 
    public ICollection<MongoDBTicket> TicketIds { get; set; }
}