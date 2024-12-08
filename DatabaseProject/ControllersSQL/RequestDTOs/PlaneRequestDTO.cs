using Database_project.Core.SQL.Entities;

namespace Database_project.Controllers.RequestDTOs
{
    public class PlaneRequestDTO
    {
        public string PlaneDisplayName { get; set; }
        public long? AirlineId { get; set; }
    }
}