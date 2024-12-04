using Database_project.Core.DTOs;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core.Services;

public class PlaneService : IPlaneService
{
    private readonly IDbContextFactory<DatabaseContext> _context;

    public PlaneService(IDbContextFactory<DatabaseContext> context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlaneDTO>> GetPlanesAsync()
    {
        await using var context = await _context.CreateDbContextAsync();

        var planes = await context.Planes
               .Include(p => p.Airline) // Include the related Airline
               .AsNoTracking()
               .ToListAsync();

        var planeDTOs = planes.Select(plane => new PlaneDTO
        {
            PlaneId = plane.PlaneId,
            PlaneDisplayName = plane.PlaneDisplayName,
            Airline = plane.Airline != null ? new AirlineDTO_Plane
            {
                AirlineId = plane.Airline.AirlineId,
                AirlineName = plane.Airline.AirlineName
            } : null
        });

        return planeDTOs;
    }

    public async Task<PlaneDTO?> GetPlaneByIdAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();

        //Get Plane object with nested object Airline
        var plane = await context.Planes
            .Include(a => a.Airline)
            .FirstOrDefaultAsync(a => a.PlaneId == id);

        if (plane == null)
        {
            throw new KeyNotFoundException($"Plane with ID {id} not found.");
        }

        //Map Plane object to PlaneDTO object
        var planeDTO = new PlaneDTO()
        {
            PlaneId = plane.PlaneId,
            PlaneDisplayName = plane.PlaneDisplayName,
            Airline = new AirlineDTO_Plane()
            {
                AirlineId = plane.Airline.AirlineId,
                AirlineName = plane.Airline.AirlineName
            }
        };
        return planeDTO;
    }

    public async Task<PlaneDTO> CreatePlaneAsync(Plane plane)
    {
        await using var context = await _context.CreateDbContextAsync();

        plane.Airline = await context.Airlines.FindAsync(plane.AirlineId);

        if (plane.Airline == null)
        {
            throw new KeyNotFoundException($"Assigned airline not found");
        }

        await context.Planes.AddAsync(plane);
        await context.SaveChangesAsync();

        return GetPlaneByIdAsync(plane.PlaneId).Result;
    }

    public async Task<PlaneDTO> UpdatePlaneAsync(Plane plane)
    {
        await using var context = await _context.CreateDbContextAsync();

        // Retrieve the existing plane from the database
        var existingPlane = await context.Planes.FindAsync(plane.PlaneId);
        if (existingPlane == null)
        {
            throw new KeyNotFoundException($"Plane with ID {plane.PlaneId} not found.");
        }

        // Check if the airline exists
        var airline = await context.Airlines.FindAsync(plane.AirlineId);
        if (airline == null)
        {
            throw new KeyNotFoundException($"Airline with ID {plane.AirlineId} not found.");
        }

        existingPlane.AirlineId = plane.AirlineId;
        existingPlane.PlaneDisplayName = plane.PlaneDisplayName;

        context.Planes.Update(existingPlane);
        await context.SaveChangesAsync();

        return GetPlaneByIdAsync(plane.PlaneId).Result;
    }

    public async Task<Plane> DeletePlaneAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();

        var plane = await context.Planes.FindAsync(id);
        if (plane == null)
        {
            throw new KeyNotFoundException($"Plane with ID {id} not found.");
        }
        var returnEntityEntry = context.Planes.Remove(plane);
        await context.SaveChangesAsync();

        return returnEntityEntry.Entity;
    }
}

