using Database_project.Core.SQL.DTOs;
using Database_project.Core.SQL.Entities;
using Database_project.Controllers.RequestDTOs;

namespace Database_project.Core.SQL.Interfaces;

public interface ITicketService
{
    Task<TicketDTO?> GetTicketByIdAsync(long id);
    Task<TicketDTO> CreateTicketAsync(Ticket ticket);
    Task<TicketDTO> UpdateTicketAsync(Ticket ticket);
    Task<Ticket> DeleteTicketAsync(long id);
}