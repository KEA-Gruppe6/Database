using Database_project.Core.MongoDB.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Database_project.Core.MongoDB.Services;

public class DepartureService
{
    private readonly IMongoCollection<MongoDBDeparture> _departureCollection;
    
    public DepartureService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _departureCollection = database.GetCollection<MongoDBDeparture>("Departure");
    }
    
    public async Task<MongoDBDeparture?> GetDepartureByIdAsync(string id)
    {
        return await Task.Run(() =>
            _departureCollection.AsQueryable()
                .FirstOrDefault(c => c.DepartureId == id));
    }
    
    public async Task<List<MongoDBDeparture>> GetDepartureAsync()
    {
        return await Task.Run(() =>
            _departureCollection.AsQueryable().ToList());
    }

    public async Task CreateDepartureAsync(MongoDBDeparture departure)
    {
        await _departureCollection.InsertOneAsync(departure);
    }
/*
 *  public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string DepartureAirportId { get; set; }
    public string ArrivalAirportId { get; set; }
    public ICollection<MongoDBTicket> TicketIds { get; set; }
 */
    public async Task UpdateDepartureAsync(string id, MongoDBDeparture updatedDeparture)
    {
        var updateDefinition = Builders<MongoDBDeparture>.Update
            .Set(c => c.DepartureTime, updatedDeparture.DepartureTime)
            .Set(c => c.ArrivalTime, updatedDeparture.ArrivalTime)
            .Set(c => c.DepartureAirportId, updatedDeparture.DepartureAirportId)
            .Set(c => c.ArrivalAirportId, updatedDeparture.ArrivalAirportId)
            .Set(c => c.TicketIds, updatedDeparture.TicketIds);

        var result = await _departureCollection.UpdateOneAsync(
            c => c.DepartureId == id,
            updateDefinition);
        
        if (result.ModifiedCount == 0)
        {
            throw new Exception($"Departure with ID {id} not found or no changes were made.");
        }
    }

    public async Task DeleteDepartureAsync(string id)
    {
        var departure = await _departureCollection.AsQueryable()
            .FirstOrDefaultAsync(c => c.DepartureId == id);

        if (departure != null)
        {
            await _departureCollection.DeleteOneAsync(c => c.DepartureId == id);
        }
    }
}