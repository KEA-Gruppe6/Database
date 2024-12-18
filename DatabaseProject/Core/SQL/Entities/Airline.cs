﻿using System.ComponentModel.DataAnnotations;

namespace Database_project.Core.SQL.Entities;

public class Airline
{
    [Key]
    public long AirlineId { get; set; }
    [MaxLength(255)]
    public string AirlineName { get; set; }
    public ICollection<Plane> Planes { get; set; }
}