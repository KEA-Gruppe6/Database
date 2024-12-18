using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Database_project.Core.SQL.Entities;

public class Customer
{
    [Key]
    public long CustomerId { get; set; }
    [MaxLength(255)]
    public string FirstName { get; set; }
    [MaxLength(255)]
    public string LastName { get; set; }
    public int PassportNumber { get; set; }
    [JsonIgnore]
    public DateTime CreatedDate { get; set; } // Audit column
    [JsonIgnore]
    public string CreatedBy { get; set; } // Audit column
    [JsonIgnore]
    public DateTime? ModifiedDate { get; set; } // Audit column
    [JsonIgnore]
    public string? ModifiedBy { get; set; } // Audit column
}