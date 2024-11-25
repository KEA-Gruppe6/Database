using Database_project.Core.MongoDB.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Database_project.Core.MongoDB.Services;

public class AirlineService // : IAirlineService
{
    private readonly IMongoCollection<MongoDBAirline> _airlineCollection;
    public AirlineService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _airlineCollection = database.GetCollection<MongoDBAirline>("Airline");
    }
    
    
    public async Task<MongoDBAirline?> GetAirlineByIdAsync(string airlineId)
    {
        return await Task.Run(() =>
            _airlineCollection.AsQueryable()
                .FirstOrDefault(c => c.AirlineId == airlineId));
    }
    public async Task<List<MongoDBAirline>> GetAirlineAsync()
    {
        return await Task.Run(() =>
            _airlineCollection.AsQueryable().ToList());
    }
    public async Task CreateAirlineAsync(MongoDBAirline airline)
    {
        await _airlineCollection.InsertOneAsync(airline);
    }
/*
 *  public long AirlineId { get; set; }
    public string AirlineName { get; set; }

    public ICollection<Plane> Planes { get; set; }
 */
    public async Task UpdateAirlineAsync(string airlineId, MongoDBAirline updatedAirline)
    {
        var updateDefinition = Builders<MongoDBAirline>.Update
            .Set(c => c.AirlineName, updatedAirline.AirlineName)
            .Set(c => c.Planes, updatedAirline.Planes);

        var result = await _airlineCollection.UpdateOneAsync(
            c => c.AirlineId == airlineId,
            updateDefinition);
        
        if (result.ModifiedCount == 0)
        {
            throw new Exception($"Airline with ID {airlineId} not found or no changes were made.");
        }
    }

    public async Task DeleteAirlineAsync(string airlineId)
    {
        await Task.Run(() =>
        {
            var customer = _airlineCollection.AsQueryable()
                .FirstOrDefault(c => c.AirlineId == airlineId);

            if (customer != null)
            {
                _airlineCollection.DeleteOne(c => c.AirlineId == airlineId);
            }
        });
    }
}