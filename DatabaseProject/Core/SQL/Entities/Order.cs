using System.ComponentModel.DataAnnotations;

namespace Database_project.Core.SQL.Entities;

public class Order
{
    [Key]
    public long OrderId { get; set; }
    [MaxLength(255)]
    public string AirlineConfirmationNumber { get; set; }

    public ICollection<Ticket> Tickets { get; set; }
}