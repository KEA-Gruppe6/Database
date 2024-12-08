using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Database_project.Core.MongoDB.Services;

public class MongoDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        _database = mongoClient.GetDatabase(settings.Value.DatabaseName);
    }

    
    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}