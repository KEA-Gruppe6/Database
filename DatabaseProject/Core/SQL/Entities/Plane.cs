﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Database_project.Core.SQL.Entities;

public class Plane
{
    [Key]
    public long PlaneId { get; set; }
    [MaxLength(255)]
    public string PlaneDisplayName { get; set; }

    public long? AirlineId { get; set; }
    [JsonIgnore]
    public Airline? Airline { get; set; }
    [JsonIgnore]

    public ICollection<Flightroute> Flightroutes { get; set; }

}