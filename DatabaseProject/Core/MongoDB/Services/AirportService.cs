using Database_project.Core.MongoDB.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Database_project.Core.MongoDB.Services;

public class AirportService
{
    private readonly IMongoCollection<MongoDBAirport> _airportCollection;
    
    public AirportService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _airportCollection = database.GetCollection<MongoDBAirport>("Airport");
    }
    
    public async Task<MongoDBAirport?> GetAirportByIdAsync(string id)
    {
        return await Task.Run(() =>
            _airportCollection.AsQueryable()
                .FirstOrDefault(c => c.AirportId == id));
    }
    public async Task<List<MongoDBAirport>> GetAirportAsync()
    {
        return await Task.Run(() =>
            _airportCollection.AsQueryable().ToList());
    }

    public async Task CreateAirportAsync(MongoDBAirport airport)
    {
        await _airportCollection.InsertOneAsync(airport);
    }
/*
 *  public string AirportName { get; set; }
    public string AirportCity { get; set; }
    public string Municipality { get; set; }
    public string AirportAbbreviation { get; set; }
 */
    public async Task UpdateAirportAsync(string id, MongoDBAirport updatedAirport)
    {
        var updateDefinition = Builders<MongoDBAirport>.Update
            .Set(c => c.AirportName, updatedAirport.AirportName)
            .Set(c => c.AirportCity, updatedAirport.AirportCity)
            .Set(c => c.Municipality, updatedAirport.Municipality)
            .Set(c => c.AirportAbbreviation, updatedAirport.AirportAbbreviation);

        var result = await _airportCollection.UpdateOneAsync(
            c => c.AirportId == id,
            updateDefinition);
        
        if (result.ModifiedCount == 0)
        {
            throw new Exception($"Airport with ID {id} not found or no changes were made.");
        }
    }

    public async Task DeleteAirportAsync(string id)
    {
        await Task.Run(() =>
        {
            var airport = _airportCollection.AsQueryable()
                .FirstOrDefault(c => c.AirportId == id);

            if (airport != null)
            {
                _airportCollection.DeleteOne(c => c.AirportId == id);
            }
        });
    }
}