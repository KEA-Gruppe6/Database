using System.ComponentModel.DataAnnotations;

namespace Database_project.Controllers.RequestDTOs;

public class CustomerRequestDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int PassportNumber { get; set; }
}