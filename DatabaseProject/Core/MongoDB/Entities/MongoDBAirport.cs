using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database_project.Core.MongoDB.Entities;

public class MongoDBAirport
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string AirportId { get; set; }

    [BsonElement("AirportName")]
    public string AirportName { get; set; }

    [BsonElement("AirportCity")]
    public string AirportCity { get; set; }

    [BsonElement("Municipality")]
    public string Municipality { get; set; }

    [BsonElement("AirportAbbreviation")]
    public string AirportAbbreviation { get; set; }
}