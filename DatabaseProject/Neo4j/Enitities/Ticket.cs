using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Database_project.Neo4j.Entities;

public class Ticket
{
    [Key]
    public long TicketId { get; set; }
    public double Price { get; set; }
    public TicketType TicketType { get; set; }

    public Customer Customer { get; set; }

    public Flightroute Flightroute { get; set; }

    public ICollection<Luggage> Luggage { get; set; }
}