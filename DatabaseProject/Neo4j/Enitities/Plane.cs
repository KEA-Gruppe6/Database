using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Database_project.Neo4j.Entities;

public class Plane
{
    [Key]
    public long PlaneId { get; set; }
    public string PlaneDisplayName { get; set; }
}