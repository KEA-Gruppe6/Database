using System.ComponentModel.DataAnnotations;

namespace Database_project.Controllers.RequestDTOs;
public class AirportRequestDTO
{
    public string AirportName { get; set; }
    public string AirportCity { get; set; }
    public string Municipality { get; set; }
    public string AirportAbbreviation { get; set; }
}