using Database_project.Core.DTOs;
using Database_project.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core.Services
{
    public class PlaneService
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

        public async Task<Plane?> UpdatePlaneAsync(Plane updatedPlane)
        {
            await using var context = await _context.CreateDbContextAsync();    
            var planeToBeUpdated =  await context.Planes.SingleOrDefaultAsync(p => p.PlaneId == updatedPlane.PlaneId);   

            if (planeToBeUpdated == null)
            {
                throw new Exception($"Could not find Plane with id: {updatedPlane.PlaneId}");
            }

            planeToBeUpdated.AirlineId = updatedPlane.AirlineId;
            planeToBeUpdated.PlaneDisplayName = updatedPlane.PlaneDisplayName;

            await context.SaveChangesAsync();
            return planeToBeUpdated;
        }

        public async Task<bool> DeletePlaneByIdAsync(long planeId)
        {
            await using var context = await _context.CreateDbContextAsync();

            var planeToBeDeleted = await context.Planes.SingleOrDefaultAsync(p => p.PlaneId == planeId);

            if (planeToBeDeleted == null)
                return true;

            context.Planes.Remove(planeToBeDeleted);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
