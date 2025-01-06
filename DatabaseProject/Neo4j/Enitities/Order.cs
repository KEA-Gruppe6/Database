using System.ComponentModel.DataAnnotations;

namespace Database_project.Neo4j.Entities;

public class Order
{
    [Key]
    public long OrderId { get; set; }
    public string AirlineConfirmationNumber { get; set; }

    public ICollection<Ticket> Tickets { get; set; }
}