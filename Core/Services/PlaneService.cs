using Database_project.Core.DTOs;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core.Services
{
    public class PlaneService : IPlaneService
    {
        private readonly IDbContextFactory<DatabaseContext> _context;

        public PlaneService(IDbContextFactory<DatabaseContext> context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Plane>> GetPlanesAsync()
        {
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var planes = await context.Planes.AsNoTracking()
                    .ToListAsync();

                return planes;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception();
            }
        }

        public async Task<Plane?> GetPlaneByIdAsync(long id)
        {
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var plane = await context.Planes.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PlaneId == id);

                if (plane == null)
                {
                    throw new Exception($"Could not find Plane with id: {id}");
                }

                return plane;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception();
            }
        }

        public async Task<Plane> CreatePlaneAsync(Plane plane)
        {
            plane.PlaneId = 0;
            await using var context = await _context.CreateDbContextAsync();
            var createdPlane = await context.Planes.AddAsync(plane);
            await context.SaveChangesAsync();

            return createdPlane.Entity;
        }

        public async Task<bool> UpdatePlaneAsync(Plane updatedPlane)
        {
            await using var context = await _context.CreateDbContextAsync();
            var planeToBeUpdated = await context.Planes.SingleOrDefaultAsync(p => p.PlaneId == updatedPlane.PlaneId);

            if (planeToBeUpdated == null)
            {
                throw new Exception($"Could not find Plane with id: {updatedPlane.PlaneId}");
            }

            planeToBeUpdated.AirlineId = updatedPlane.AirlineId;
            planeToBeUpdated.PlaneDisplayName = updatedPlane.PlaneDisplayName;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePlaneAsync(long id)
        {
            await using var context = await _context.CreateDbContextAsync();

            var planeToBeDeleted = await context.Planes.SingleOrDefaultAsync(p => p.PlaneId == id);

            if (planeToBeDeleted == null)
                return true;

            context.Planes.Remove(planeToBeDeleted);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
