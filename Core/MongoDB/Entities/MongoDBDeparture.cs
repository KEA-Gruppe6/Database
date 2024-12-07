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
    public string DepartureAirportId { get; set; }

    [BsonElement("ArrivalAirportId")]
    public string ArrivalAirportId { get; set; }

    [BsonElement("Tickets")]
    public ICollection<MongoDBTicket> TicketIds { get; set; }
}