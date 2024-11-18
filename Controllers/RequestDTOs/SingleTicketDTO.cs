using Database_project.Core.Entities;

namespace Database_project.Controllers.RequestDTOs
{
    public class SingleTicketDTO
    {
        public long TicketTypeId { get; set; }
        public Customer? Customer { get; set; }
    }
}
