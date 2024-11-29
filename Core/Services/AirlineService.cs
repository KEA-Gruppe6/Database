using Database_project.Core;
using Database_project.Core.DTOs;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Services;

public class AirlineService : IAirlineService
{
    private readonly IDbContextFactory<DatabaseContext> _context;

    public AirlineService(IDbContextFactory<DatabaseContext> context)
    {
        _context = context;
    }

    public async Task<AirlineDTO?> GetAirlineByIdAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();

        var airline = await context.Airlines
            .Select(a => new AirlineDTO()
            {
                AirlineId = a.AirlineId,
                AirlineName = a.AirlineName,
                Planes = a.Planes.Select(p => new PlaneDTO()
                {
                    PlaneId = p.PlaneId,
                    PlaneDisplayName = p.PlaneDisplayName
                }).ToList()
            })
            .FirstOrDefaultAsync(a => a.AirlineId == id);

        return airline;
    }

    public async Task<Airline> CreateAirlineAsync(Airline airline)
    {
        await using var context = await _context.CreateDbContextAsync();

        await context.Airlines.AddAsync(airline);
        await context.SaveChangesAsync();

        return airline;
    }

    public async Task<bool> UpdateAirlineAsync(Airline airline)
    {
        await using var context = await _context.CreateDbContextAsync();
        try
        {
            var existingAirline = await context.Airlines
                .Include(a => a.Planes)  // Include planes to handle updates
                .FirstOrDefaultAsync(a => a.AirlineId == airline.AirlineId);

            if (existingAirline == null)
            {
                return false;  // If the airline doesn't exist, return false
            }
            //change airline name 
            existingAirline.AirlineName = airline.AirlineName;

            //Remove Planes
            var planeIdsToRemove = existingAirline.Planes
                .Where(p => !airline.Planes.Any(updatedPlane => updatedPlane.PlaneId == p.PlaneId))
                .Select(p => p.PlaneId)
                .ToList();

            foreach (var planeId in planeIdsToRemove)
            {
                var planeToRemove = existingAirline.Planes.First(p => p.PlaneId == planeId);
                existingAirline.Planes.Remove(planeToRemove);
            }

            //Add planes
            var planeIdsToAdd = airline.Planes
                .Where(p => !existingAirline.Planes.Any(existingPlane => existingPlane.PlaneId == p.PlaneId))
                .ToList();

            foreach (var newPlane in planeIdsToAdd)
            {
                existingAirline.Planes.Add(new Plane
                {
                    PlaneId = newPlane.PlaneId,
                    PlaneDisplayName = newPlane.PlaneDisplayName,
                    AirlineId = airline.AirlineId // Ensure the new plane is associated with the updated airline
                });
            }

            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception();
        }
    }

    public async Task<bool> DeleteAirlineAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();
        try
        {
            var airline = await context.Airlines.Include(a => a.Planes).FirstOrDefaultAsync(a => a.AirlineId == id);
            if (airline == null)
            {
                return false;
            }

            // Remove related planes
            context.Planes.RemoveRange(airline.Planes);

            // Remove the airline
            context.Airlines.Remove(airline);

            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the airline.", ex);
        }
    }
}