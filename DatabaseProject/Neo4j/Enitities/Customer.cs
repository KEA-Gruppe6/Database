using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Database_project.Neo4j.Entities;

public class Customer
{
    [Key]
    public long CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int PassportNumber { get; set; }
    [JsonIgnore]
    public bool IsDeleted { get; set; } // Soft delete column
}