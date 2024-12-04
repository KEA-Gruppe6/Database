using Database_project.Core;
using Database_project.Core.DTOs;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Services;

public class MaintenanceService : IMaintenanceService
{
    private readonly IDbContextFactory<DatabaseContext> _context;

    public MaintenanceService(IDbContextFactory<DatabaseContext> context)
    {
        _context = context;
    }

    private async Task ValidateEntitiesExistenceAsync(Maintenance maintenance, DatabaseContext context)
    {
        Plane plane = await context.Planes.FirstOrDefaultAsync(a => a.PlaneId == maintenance.PlaneId);
        if (plane == null)
        {
            throw new KeyNotFoundException($"Plane with ID {maintenance.PlaneId} not found.");
        }

        Airport airport = await context.Airports.FirstOrDefaultAsync(a => a.AirportId == maintenance.AirportId);
        if (airport == null)
        {
            throw new KeyNotFoundException($"Airport with ID {maintenance.AirportId} not found.");
        }
    }

    public async Task<MaintenanceDTO?> GetMaintenanceByIdAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();

        //Get Maintenance object with nested objects Plane and Airport
        var maintenance = await context.Maintenances
            .Include(m => m.Plane)
            .Include(m => m.Airport)
            .FirstOrDefaultAsync(a => a.MaintenanceId == id);

        if (maintenance == null)
        {
            throw new KeyNotFoundException($"Maintenance with ID {id} not found.");
        }

        //Map Maintenance object to MaintenanceDTO object
        var maintenanceDTO = new MaintenanceDTO()
        {
            MaintenanceId = maintenance.MaintenanceId,
            StartDate = maintenance.StartDate,
            EndDate = maintenance.EndDate,
            Plane = new PlaneDTO()
            {
                PlaneId = maintenance.Plane.PlaneId,
                PlaneDisplayName = maintenance.Plane.PlaneDisplayName
            },
            Airport = maintenance.Airport
        };
        return maintenanceDTO;
    }

    public async Task<MaintenanceDTO> CreateMaintenanceAsync(Maintenance maintenance)
    {
        await using var context = await _context.CreateDbContextAsync();

        await ValidateEntitiesExistenceAsync(maintenance, context);

        context.Maintenances.AddAsync(maintenance);
        await context.SaveChangesAsync();

        return GetMaintenanceByIdAsync(maintenance.MaintenanceId).Result;
    }

    public async Task<MaintenanceDTO> UpdateMaintenanceAsync(Maintenance maintenance)
    {
        await using var context = await _context.CreateDbContextAsync();

        // Retrieve the existing maintenance from the database
        var existingMaintenance = await context.Maintenances.FindAsync(maintenance.MaintenanceId);
        if (existingMaintenance == null)
        {
            throw new KeyNotFoundException($"Maintenance with ID {maintenance.MaintenanceId} not found.");
        }

        await ValidateEntitiesExistenceAsync(maintenance, context);

        // Update the properties of the existing maintenance
        existingMaintenance.StartDate = maintenance.StartDate;
        existingMaintenance.EndDate = maintenance.EndDate;
        existingMaintenance.PlaneId = maintenance.PlaneId;
        existingMaintenance.AirportId = maintenance.AirportId;

        // Save the changes to the database
        context.Maintenances.Update(existingMaintenance);
        await context.SaveChangesAsync();

        return GetMaintenanceByIdAsync(maintenance.MaintenanceId).Result;
    }

    public async Task<Maintenance> DeleteMaintenanceAsync(long id)
    {
        {
            await using var context = await _context.CreateDbContextAsync();

            var maintenance = await context.Maintenances.FindAsync(id);
            if (maintenance == null)
            {
                throw new KeyNotFoundException($"Maintenancee with ID {id} not found.");
            }
            var returnEntityEntry = context.Maintenances.Remove(maintenance);
            await context.SaveChangesAsync();

            return returnEntityEntry.Entity;
        }
    }
}