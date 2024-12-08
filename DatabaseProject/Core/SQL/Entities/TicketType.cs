using System.ComponentModel.DataAnnotations;

namespace Database_project.Core.SQL.Entities;

public class TicketType
{
    [Key]
    public long TicketTypeId { get; set; }
    public string TicketTypeName { get; set; }
}