using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database_project.Core.MongoDB.Entities;

public class MongoDBDeparture
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] 
    public string DepartureId { get; set; }

    [BsonElement("DepartureTime")]
    public DateTime DepartureTime { get; set; }

    [BsonElement("ArrivalTime")]
    public DateTime ArrivalTime { get; set; }

    [BsonElement("DepartureAirportId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string DepartureAirportId { get; set; }

    [BsonElement("ArrivalAirportId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ArrivalAirportId { get; set; }

    [BsonElement("Tickets")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ICollection<MongoDBTicket> TicketIds { get; set; }
}