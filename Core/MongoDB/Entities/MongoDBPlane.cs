﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database_project.Core.MongoDB.Entities;

public class MongoDBPlane
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PlaneId { get; set; }
    
    [BsonElement("PlaneDisplayName")]
    public string PlaneDisplayName { get; set; }
    
    [BsonElement("AirlineId")]
    public string AirlineId { get; set; }
    
    [BsonElement("Departures")]
    public ICollection<MongoDBDeparture> Departures { get; set; } = new List<MongoDBDeparture>();
}
    

    