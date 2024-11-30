using System.ComponentModel.DataAnnotations;

namespace Database_project.Core.Entities;

public class Luggage
{
    [Key]
    public long LuggageId { get; set; }
    public double MaxWeight { get; set; }
    public bool IsCarryOn { get; set; }
    public long TicketId { get; set; }

}