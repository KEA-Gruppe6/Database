using System.ComponentModel.DataAnnotations;

namespace Database_project.Core.Entities;

public class Order
{
    [Key]
    public int OrderId { get; set; } //TODO: Make long
    public string AirlineConfirmationNumber { get; set; }

    public ICollection<Ticket> Tickets { get; set; }
}