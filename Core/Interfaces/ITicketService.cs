using Database_project.Core.DTOs;
using Database_project.Core.Entities;
using Database_project.Controllers.RequestDTOs;

namespace Database_project.Core.Interfaces;

public interface ITicketService
{
    Task<TicketDTO?> GetTicketByIdAsync(long id);
    Task<TicketDTO> CreateTicketAsync(Ticket ticket);
    Task<TicketDTO> UpdateTicketAsync(Ticket ticket);
    Task<Ticket> DeleteTicketAsync(long id);
}