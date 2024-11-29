using Database_project.Core.DTOs;
using Database_project.Core.Entities;

namespace Database_project.Core.Interfaces;

public interface IPlaneService
{
    Task<IEnumerable<Plane>> GetPlanesAsync();
    Task<Plane?> GetPlaneByIdAsync(long id);
    Task<Plane> CreatePlaneAsync(Plane plane);
    Task<bool> UpdatePlaneAsync(Plane updatedPlane);
    Task<bool> DeletePlaneAsync(long id);
}