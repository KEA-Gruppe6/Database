using System.ComponentModel.DataAnnotations;

namespace Database_project.Controllers.RequestDTOs;

public class AirlineRequestDTO
{
    public long AirlineId { get; set; }
    public string AirlineName { get; set; }
}