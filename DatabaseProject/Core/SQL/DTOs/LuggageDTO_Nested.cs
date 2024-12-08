using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database_project.Core.SQL.Entities;

public class LuggageDTO_Nested
{
    public double Weight { get; set; }
    public bool IsCarryOn { get; set; }
}