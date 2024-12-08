using Database_project.Core.SQL.DTOs;
using Database_project.Core.SQL.Entities;

namespace Database_project.Core.SQL.Interfaces;

public interface ITicketTypeService
{
    Task<List<TicketType>> GetTicketTypesAsync();
    Task<TicketType?> GetTicketTypeByIdAsync(long id);
    Task<TicketType> CreateTicketTypeAsync(TicketType ticketType);
    Task<TicketType> UpdateTicketTypeAsync(TicketType ticketType);
    Task<TicketType> DeleteTicketTypeAsync(long id);
}