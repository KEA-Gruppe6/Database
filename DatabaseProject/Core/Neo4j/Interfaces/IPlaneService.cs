using Database_project.Core.Neo4j.Entities;

namespace Database_project.Core.Neo4j.Interfaces;

public interface IPlaneService
{
    Task<IEnumerable<Plane>> GetPlanesAsync();
    Task<Plane?> GetPlaneByIdAsync(long id);
    Task<Plane> CreatePlaneAsync(Plane plane);
    Task<Plane> UpdatePlaneAsync(long id, Plane plane);
    Task<Plane> DeletePlaneAsync(long id);
}