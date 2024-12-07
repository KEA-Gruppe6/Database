using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_project.Core.Entities;

public class LuggageDTO
{
    public double Weight { get; set; }
    public bool IsCarryOn { get; set; }
}