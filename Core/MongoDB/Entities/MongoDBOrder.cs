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
    public ICollection<string> TicketIds { get; set; }
    
    public MongoDBOrder()
    {
        AirlineConfirmationNumber = GenerateConfirmationNumber();
        TicketIds = new List<string>();
    }
    
    public static string GenerateConfirmationNumber()
    {
        return Guid.NewGuid().ToString().Substring(0, 8).ToUpper(); // Example: "ABC12345"
    }

    public void addTicket(MongoDBTicket ticket)
    {
        TicketIds.Add(ticket.TicketId);
    }

    public void removeTicket(MongoDBTicket ticket)
    {
        if (TicketIds.Contains(ticket.TicketId))
        {
            TicketIds.Remove(ticket.TicketId);
        }
    }
}