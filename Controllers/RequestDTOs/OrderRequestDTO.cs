using Database_project.Core.Entities;

namespace Database_project.Controllers.RequestDTOs
{
    public class OrderRequestDTO
    {
        public string AirlineConfirmationNumber { get; set; }
        public Ticket[] Tickets { get; set; }
    }
}
