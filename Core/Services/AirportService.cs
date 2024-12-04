using Database_project.Core;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Services;

public class AirportService : IAirportService
{
    private readonly IDbContextFactory<DatabaseContext> _context;

    public AirportService(IDbContextFactory<DatabaseContext> context)
    {
        _context = context;
    }

    public async Task<Airport?> GetAirportByIdAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();
        var airport = await context.Airports.FirstOrDefaultAsync(a => a.AirportId == id);

        return airport;
    }

    public async Task<Airport> CreateAirportAsync(Airport airport)
    {
        await using var context = await _context.CreateDbContextAsync();

        await context.Airports.AddAsync(airport);
        await context.SaveChangesAsync();

        return airport;
    }

    public async Task<Airport> UpdateAirportAsync(Airport airport)
    {
        await using var context = await _context.CreateDbContextAsync();

        // Retrieve the existing airport from the database
        var existingAirport = await context.Airports.FindAsync(airport.AirportId);
        if (existingAirport == null)
        {
            throw new KeyNotFoundException($"Airport with ID {airport.AirportId} not found.");
        }

        existingAirport.AirportName = airport.AirportName;
        existingAirport.AirportCity = airport.AirportCity;
        existingAirport.Municipality = airport.Municipality;
        existingAirport.AirportAbbreviation = airport.AirportAbbreviation;

        var returnEntityEntry = context.Airports.Update(existingAirport);
        await context.SaveChangesAsync();

        return returnEntityEntry.Entity;
    }

    public async Task<Airport> DeleteAirportAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();

        var airport = await context.Airports.FindAsync(id);
        if (airport == null)
        {
            throw new KeyNotFoundException($"Airport with ID {id} not found.");
        }
        var returnEntityEntry = context.Airports.Remove(airport);
        await context.SaveChangesAsync();

        return returnEntityEntry.Entity;
    }
}