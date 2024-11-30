using Database_project.Core.Entities;

namespace Database_project.Controllers.RequestDTOs
{
    public class OrderRequestDTO
    {
        public long FlightrouteId { get; set; }
        public List<SingleTicketDTO>? Tickets { get; set; }
    }
}
