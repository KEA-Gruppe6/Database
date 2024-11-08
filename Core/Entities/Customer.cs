using System.ComponentModel.DataAnnotations;

namespace Database_project.Core.Entities;

public class Customer
{
    [Key] public long CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int PassportNumber { get; set; }
}