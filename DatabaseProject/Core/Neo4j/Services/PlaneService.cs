using Database_project.Core.Neo4j;
using Database_project.Core.Neo4j.Entities;
using Database_project.Core.Neo4j.Interfaces;
using Neo4jClient;

namespace Database_project.Core.Neo4j.Services;

public class PlaneService : IPlaneService
{
    private readonly IGraphClient _neo4jClient;

    public PlaneService(IGraphClient neo4jClient)
    {
        _neo4jClient = neo4jClient;
    }

    public async Task<IEnumerable<Plane>> GetPlanesAsync()
    {
        var planes = await _neo4jClient.Cypher
            .Match("(plane:Plane)")
            .Return(plane => plane.As<Plane>())
            .ResultsAsync;

        return planes;
    }

    public async Task<Plane?> GetPlaneByIdAsync(long id)
    {
        var plane = await _neo4jClient.Cypher
            .Match("(plane:Plane)")
            .Where((Plane plane) => plane.Id == id)
            .Return(plane => plane.As<Plane>())
            .ResultsAsync;

        var planeResult = plane.FirstOrDefault();

        if (planeResult == null)
        {
            throw new KeyNotFoundException($"Plane with ID {id} not found.");
        }

        return planeResult;
    }

    public async Task<Plane> CreatePlaneAsync(Plane plane)
    {
        await _neo4jClient.Cypher //TODO create reference to airline
            .Create("(plane:Plane $newPlane)")
            .WithParam("newPlane", plane)
            .ExecuteWithoutResultsAsync();

        return GetPlaneByIdAsync(plane.Id).Result;
    }

    public async Task<Plane> UpdatePlaneAsync(long id, Plane plane)
    {
        await _neo4jClient.Cypher
            .Match("(plane:Plane)")
            .Where((Plane plane) => plane.Id == id)
            .Set("plane = $newPlane")
            .WithParam("newPlane", plane)
            .ExecuteWithoutResultsAsync();

        return GetPlaneByIdAsync(plane.Id).Result;
    }

    public async Task<Plane> DeletePlaneAsync(long id)
    {
        var deletedPlane = GetPlaneByIdAsync(id).Result;

        await _neo4jClient.Cypher
            .Match("(plane:Plane)")
            .Where((Plane plane) => plane.Id == id)
            .Delete("plane")
            .ExecuteWithoutResultsAsync();

        return deletedPlane;
    }
}

