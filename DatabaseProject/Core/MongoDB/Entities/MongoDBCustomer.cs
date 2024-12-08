using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database_project.Core.MongoDB.Entities;

public class MongoDBCustomer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CustomerId { get; set; }

    [BsonElement("firstName")]
    public string FirstName { get; set; }

    [BsonElement("lastName")]
    public string LastName { get; set; }

    [BsonElement("passportNumber")]
    public int PassportNumber { get; set; }
}
