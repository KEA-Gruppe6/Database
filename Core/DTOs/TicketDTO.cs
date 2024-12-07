using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Database_project.Core.DTOs;

namespace Database_project.Core.Entities;

public class TicketDTO
{
    public long TicketId { get; set; }
    public double Price { get; set; }

    public TicketType TicketType { get; set; }

    public Customer Customer { get; set; }

    public FlightrouteDTO Flightroute { get; set; }

    public ICollection<LuggageDTO> Luggage { get; set; }
}