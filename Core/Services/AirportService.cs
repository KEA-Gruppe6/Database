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
        try
        {
            var airport = await context.Airports.FirstOrDefaultAsync(a => a.AirportId == id);
            
            if (airport == null)
            {
                throw new Exception($"Could not find Airline with id: {id}");
            }

            return airport;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Airport> CreateAirportAsync(Airport airport)
    {
        await using var context = await _context.CreateDbContextAsync();
        try
        {
            await context.Airports.AddAsync(airport);
            await context.SaveChangesAsync();
            
            return airport;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> UpdateAirportAsync(long id, Airport airport)
    {
        await using var context = await _context.CreateDbContextAsync();
        try
        {
            var existingAirport = await context.Airports
                .FirstOrDefaultAsync(a => a.AirportId == id);
    
            if (existingAirport == null)
            {
                return false;  // If the airline doesn't exist, return false
            }
            
            existingAirport.AirportName = airport.AirportName;
            existingAirport.AirportCity = airport.AirportCity;
            existingAirport.Municipality = airport.Municipality;
            existingAirport.AirportAbbreviation = airport.AirportAbbreviation;
            
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> DeleteAirportAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();
        try
        {
            var airport = await context.Airports.FindAsync(id);
            if (airport == null)
            {
                return false;
            }

            context.Airports.Remove(airport);
            await context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}