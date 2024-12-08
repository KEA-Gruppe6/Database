using Database_project.Core.SQL;
using Database_project.Core.SQL.Entities;
using Database_project.Core.SQL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core.Services;

public class TicketTypeService(IDbContextFactory<DatabaseContext> context) : ITicketTypeService
{
    public async Task<List<TicketType>> GetTicketTypesAsync()
    {
        await using var context1 = await context.CreateDbContextAsync();
        return await context1.TicketTypes.ToListAsync();
    }

    public async Task<TicketType> GetTicketTypeByIdAsync(long id)
    {
        await using var context1 = await context.CreateDbContextAsync();
        var ticketType = await context1.TicketTypes.FindAsync(id);
        if (ticketType == null)
        {
            throw new ArgumentException($"No ticket type with id: {id} found.");
        }

        return ticketType;
    }

    public async Task<TicketType> CreateTicketTypeAsync(TicketType ticketType)
    {
        await using var context1 = await context.CreateDbContextAsync();
        var result = context1.TicketTypes.Add(ticketType);
        await context1.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<TicketType> UpdateTicketTypeAsync(TicketType ticketType)
    {
        await using var context1 = await context.CreateDbContextAsync();
        var dbTicketType = await context1.TicketTypes.FindAsync(ticketType.TicketTypeId);
        if (dbTicketType == null)
        {
            throw new ArgumentException($"No ticket type with id: {ticketType.TicketTypeId} found.");
        }

        dbTicketType.TicketTypeName = ticketType.TicketTypeName;
        await context1.SaveChangesAsync();

        return dbTicketType;
    }

    public async Task<TicketType> DeleteTicketTypeAsync(long id)
    {
        await using var context1 = await context.CreateDbContextAsync();
        var ticketType = await context1.TicketTypes.FindAsync(id);
        if (ticketType == null)
        {
            throw new ArgumentException($"No ticket type with id: {id} found.");
        }
        var result = context1.TicketTypes.Remove(ticketType);
        await context1.SaveChangesAsync();

        return result.Entity;
    }
}