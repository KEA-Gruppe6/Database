using Database_project.Core.DTOs;
using Database_project.Core.Entities;

namespace Database_project.Core.Interfaces;

public interface IPlaneService
{
    Task<IEnumerable<PlaneDTO>> GetPlanesAsync();
    Task<PlaneDTO?> GetPlaneByIdAsync(long id);
    Task<PlaneDTO> CreatePlaneAsync(Plane plane);
    Task<PlaneDTO> UpdatePlaneAsync(Plane plane);
    Task<Plane> DeletePlaneAsync(long id);
}