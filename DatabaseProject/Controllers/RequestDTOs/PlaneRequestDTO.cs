using Database_project.Core.Entities;

namespace Database_project.Controllers.RequestDTOs
{
    public class PlaneRequestDTO
    {
        public string PlaneDisplayName { get; set; }
        public long? AirlineId { get; set; }
    }
}