using Database_project.Core.MongoDB.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Database_project.Core.MongoDB.Services;

public class MaintenanceService // : IMaintenanceService
{
    private readonly IMongoCollection<MongoDBMaintenance> _maintenanceCollection;
    public MaintenanceService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _maintenanceCollection = database.GetCollection<MongoDBMaintenance>("Maintenance");
    }
    
    
    public async Task<MongoDBMaintenance?> GetMaintenanceByIdAsync(string maintenanceId)
    {
        return await Task.Run(() =>
            _maintenanceCollection.AsQueryable()
                .FirstOrDefault(c => c.MaintenanceId == maintenanceId));
    }
    public async Task<List<MongoDBMaintenance>> GetMaintenanceAsync()
    {
        return await Task.Run(() =>
            _maintenanceCollection.AsQueryable().ToList());
    }
    public async Task CreateMaintenanceAsync(MongoDBMaintenance maintenance)
    {
        await _maintenanceCollection.InsertOneAsync(maintenance);
    }
/*
 *  public long MaintenanceId { get; set; }
    public string MaintenanceName { get; set; }

    public ICollection<Plane> Planes { get; set; }
 */
    public async Task UpdateMaintenanceAsync(string maintenanceId, MongoDBMaintenance updatedMaintenance)
    {
        var updateDefinition = Builders<MongoDBMaintenance>.Update
            .Set(c => c.AirportId, updatedMaintenance.AirportId)
            .Set(c => c.PlaneId, updatedMaintenance.PlaneId);

        var result = await _maintenanceCollection.UpdateOneAsync(
            c => c.MaintenanceId == maintenanceId,
            updateDefinition);
        
        if (result.ModifiedCount == 0)
        {
            throw new Exception($"Maintenance with ID {maintenanceId} not found or no changes were made.");
        }
    }

    public async Task DeleteMaintenanceAsync(string maintenanceId)
    {
        await Task.Run(() =>
        {
            var maintenance = _maintenanceCollection.AsQueryable()
                .FirstOrDefault(c => c.MaintenanceId == maintenanceId);

            if (maintenance != null)
            {
                _maintenanceCollection.DeleteOne(c => c.MaintenanceId == maintenanceId);
            }
        });
    }
}