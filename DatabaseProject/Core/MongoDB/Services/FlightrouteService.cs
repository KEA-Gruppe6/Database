using Database_project.Core.MongoDB.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Database_project.Core.MongoDB.Services;

public class FlightrouteService
{
    private readonly IMongoCollection<MongoDBFlightroute> _flightrouteCollection;
    
    public FlightrouteService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _flightrouteCollection = database.GetCollection<MongoDBFlightroute>("Flightroute");
    }
    
    public async Task<MongoDBFlightroute?> GetFlightrouteByIdAsync(string id)
    {
        return await Task.Run(() =>
            _flightrouteCollection.AsQueryable()
                .FirstOrDefault(c => c.FlightrouteId == id));
    }
    
    public async Task<List<MongoDBFlightroute>> GetFlightrouteAsync()
    {
        return await Task.Run(() =>
            _flightrouteCollection.AsQueryable().ToList());
    }

    public async Task CreateFlightrouteAsync(MongoDBFlightroute flightroute)
    {
        await _flightrouteCollection.InsertOneAsync(flightroute);
    }

    public async Task UpdateFlightrouteAsync(string id, MongoDBFlightroute updatedFlightroute)
    {
        var updateDefinition = Builders<MongoDBFlightroute>.Update
            .Set(c => c.DepartureTime, updatedFlightroute.DepartureTime)
            .Set(c => c.ArrivalTime, updatedFlightroute.ArrivalTime)
            .Set(c => c.DepartureAirportId, updatedFlightroute.DepartureAirportId)
            .Set(c => c.ArrivalAirportId, updatedFlightroute.ArrivalAirportId)
            .Set(c => c.TicketIds, updatedFlightroute.TicketIds);

        var result = await _flightrouteCollection.UpdateOneAsync(
            c => c.FlightrouteId == id,
            updateDefinition);
        
        if (result.ModifiedCount == 0)
        {
            throw new Exception($"Flightroute with ID {id} not found or no changes were made.");
        }
    }

    public async Task DeleteFlightrouteAsync(string id)
    {
        var flightroute = await _flightrouteCollection.AsQueryable()
            .FirstOrDefaultAsync(c => c.FlightrouteId == id);

        if (flightroute != null)
        {
            await _flightrouteCollection.DeleteOneAsync(c => c.FlightrouteId == id);
        }
    }
}