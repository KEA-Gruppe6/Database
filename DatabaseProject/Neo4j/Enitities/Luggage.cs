﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_project.Neo4j.Entities;

public class Luggage
{
    [Key]
    public long LuggageId { get; set; }
    public double Weight { get; set; }
    public bool IsCarryOn { get; set; }
}