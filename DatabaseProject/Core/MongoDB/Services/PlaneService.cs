using Database_project.Core.MongoDB.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Database_project.Core.MongoDB.Services;

public class PlaneService
{
    private readonly IMongoCollection<MongoDBPlane> _planeCollection;
    
    public PlaneService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _planeCollection = database.GetCollection<MongoDBPlane>("Plane");
    }
    
    public async Task<MongoDBPlane?> GetPlaneByIdAsync(string id)
    {
        return await Task.Run(() =>
            _planeCollection.AsQueryable()
                .FirstOrDefault(c => c.PlaneId == id));
    }
    
    public async Task<List<MongoDBPlane>> GetPlaneAsync()
    {
        return await Task.Run(() =>
            _planeCollection.AsQueryable().ToList());
    }

    public async Task CreatePlaneAsync(MongoDBPlane plane)
    {
        await _planeCollection.InsertOneAsync(plane);
    }

    public async Task UpdatePlaneAsync(string id, MongoDBPlane updatedPlane)
    {
        var updateDefinition = Builders<MongoDBPlane>.Update
            .Set(c => c.PlaneDisplayName, updatedPlane.PlaneDisplayName)
            .Set(c => c.AirlineId, updatedPlane.AirlineId)
            .Set(c => c.Flightroutes, updatedPlane.Flightroutes);

        var result = await _planeCollection.UpdateOneAsync(
            c => c.PlaneId == id,
            updateDefinition);
        
        if (result.ModifiedCount == 0)
        {
            throw new Exception($"Plane with ID {id} not found or no changes were made.");
        }
    }

    public async Task DeletePlaneAsync(string id)
    {
        var plane = await _planeCollection.AsQueryable()
            .FirstOrDefaultAsync(c => c.PlaneId == id);

        if (plane != null)
        {
            await _planeCollection.DeleteOneAsync(c => c.PlaneId == id);
        }
    }
}