using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Database_project.Controllers.RequestDTOs
{
    public class TicketRequestDTO
    {
        public double Price { get; set; }

        public long TicketTypeId { get; set; }

        public long CustomerId { get; set; }

        public long FlightrouteId { get; set; }

        public long OrderId { get; set; }
    }
}