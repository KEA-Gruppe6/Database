using Database_project.Core.DTOs;
using Database_project.Core.Entities;

namespace Database_project.Core.Interfaces;

public interface IPlaneService
{
    Task<IEnumerable<PlaneDTO_Airline>> GetPlanesAsync();
    Task<PlaneDTO_Airline?> GetPlaneByIdAsync(long id);
    Task<PlaneDTO_Airline> CreatePlaneAsync(Plane plane);
    Task<PlaneDTO_Airline> UpdatePlaneAsync(Plane plane);
    Task<Plane> DeletePlaneAsync(long id);
}