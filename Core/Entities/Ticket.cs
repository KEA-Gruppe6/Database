using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_project.Core.Entities;

public class Ticket
{
    [Key]
    public long TicketId { get; set; }
    public double Price { get; set; }

    public long TicketTypeId { get; set; }
    public TicketType TicketType { get; set; }

    public long CustomerId { get; set; }
    public Customer Customer { get; set; }

    public long DepartureId { get; set; }
    public Departure Departure { get; set; }
    public int OrderId { get; set; }
    public ICollection<Luggage> Luggage { get; set; }
}