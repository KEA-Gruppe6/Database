using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database_project.Core.MongoDB.Entities;

public class MongoDBTicket
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string TicketId { get; set; }

    [BsonElement("Price")]
    public double Price { get; set; }

    [BsonElement("TicketTypeId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string TicketType { get; set; }

    [BsonElement("CustomerId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CustomerId { get; set; }

    [BsonElement("DepartureId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string DepartureId { get; set; }

    [BsonElement("OrderId")]
    public int OrderId { get; set; }

    [BsonElement("LuggageIds")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ICollection<MongoDBLuggage> LuggageIds { get; set; }
}