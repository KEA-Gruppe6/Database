using System.ComponentModel.DataAnnotations;

namespace Database_project.Controllers.RequestDTOs;

public class LuggageRequestDTO
{
    public double MaxWeight { get; set; }
    public bool IsCarryOn { get; set; }
    public long TicketId { get; set; }

}