﻿using Database_project.Core.MongoDB.Entities;

namespace Database_project.Core.MongoDB.DTO;

public class MongoDBDepartureDTO
{
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string DepartureAirportId { get; set; }
    public string ArrivalAirportId { get; set; }
    public ICollection<MongoDBTicket> TicketIds { get; set; }
 
}