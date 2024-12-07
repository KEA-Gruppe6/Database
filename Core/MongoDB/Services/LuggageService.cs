using Database_project.Core.MongoDB.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Database_project.Core.MongoDB.Services;

public class LuggageService
{
    private readonly IMongoCollection<MongoDBLuggage> _luggageCollection;
    
    public LuggageService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _luggageCollection = database.GetCollection<MongoDBLuggage>("Luggage");
    }
    
    public async Task<MongoDBLuggage?> GetLuggageByIdAsync(string id)
    {
        return await Task.Run(() =>
            _luggageCollection.AsQueryable()
                .FirstOrDefault(c => c.LuggageId == id));
    }
    public async Task<List<MongoDBLuggage>> GetLuggageAsync()
    {
        return await Task.Run(() =>
            _luggageCollection.AsQueryable().ToList());
    }

    public async Task CreateLuggageAsync(MongoDBLuggage luggage)
    {
        await _luggageCollection.InsertOneAsync(luggage);
    }

    public async Task UpdateLuggageAsync(string id, MongoDBLuggage updatedLuggage)
    {
        var updateDefinition = Builders<MongoDBLuggage>.Update
            .Set(c => c.IsCarryOn, updatedLuggage.IsCarryOn)
            .Set(c => c.MaxWeight, updatedLuggage.MaxWeight);

        var result = await _luggageCollection.UpdateOneAsync(
            c => c.LuggageId == id,
            updateDefinition);
        
        if (result.ModifiedCount == 0)
        {
            throw new Exception($"Luggage with ID {id} not found or no changes were made.");
        }
    }

    public async Task DeleteLuggageAsync(string id)
    {
        await Task.Run(() =>
        {
            var luggage = _luggageCollection.AsQueryable()
                .FirstOrDefault(c => c.LuggageId == id);

            if (luggage != null)
            {
                _luggageCollection.DeleteOne(c => c.LuggageId == id);
            }
        });
    }
}