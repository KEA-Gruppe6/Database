using Database_project.Core.DTOs;
using Database_project.Core.Entities;

namespace Database_project.Core.Interfaces;

public interface ITicketTypeService
{
    Task<List<TicketType>> GetTicketTypesAsync();
    Task<TicketType?> GetTicketTypeByIdAsync(long id);
    Task<TicketType> CreateTicketTypeAsync(TicketType ticketType);
    Task<TicketType> UpdateTicketTypeAsync(TicketType ticketType);
    Task<TicketType> DeleteTicketTypeAsync(long id);
}