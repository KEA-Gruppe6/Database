﻿using Database_project.Core.MongoDB.Entities;

namespace Database_project.Core.MongoDB.RequestDTOs;

public class MongoDBTicketDTO
{
    public double Price { get; set; }

    public string TicketType { get; set; }

    public string CustomerId { get; set; }

    public string FlightrouteId { get; set; }

    public string? OrderId { get; set; }

    public ICollection<MongoDBLuggage>? LuggageIds { get; set; }
}