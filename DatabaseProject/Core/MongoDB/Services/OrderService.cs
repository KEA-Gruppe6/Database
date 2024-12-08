using Database_project.Core.MongoDB.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Database_project.Core.MongoDB.Services;

public class OrderService
{
    private readonly IMongoCollection<MongoDBOrder> _orderCollection;
    
    public OrderService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _orderCollection = database.GetCollection<MongoDBOrder>("Order");
    }

    public async Task<MongoDBOrder?> GetOrderByIdAsync(string id)
    {
        return await Task.Run(() =>
            _orderCollection.AsQueryable()
                .FirstOrDefault(c => c.OrderId == id));
    }
    
    public async Task<List<MongoDBOrder>> GetOrdersAsync()
    {
        return await Task.Run(() =>
            _orderCollection.AsQueryable().ToList());
    }

    public async Task CreateOrderAsync(MongoDBOrder order)
    {
        await _orderCollection.InsertOneAsync(order);
    }

    public async Task UpdateOrderAsync(string id, MongoDBOrder updatedOrder)
    {
        var updateDefinition = Builders<MongoDBOrder>.Update
            .Set(c => c.AirlineConfirmationNumber, updatedOrder.AirlineConfirmationNumber)
            .Set(c => c.TicketIds, updatedOrder.TicketIds);

        var result = await _orderCollection.UpdateOneAsync(
            c => c.OrderId == id,
            updateDefinition);
        
        if (result.ModifiedCount == 0)
        {
            throw new Exception($"Order with ID {id} not found or no changes were made.");
        }
    }

    public async Task DeleteOrderAsync(string id)
    {
        await Task.Run(() =>
        {
            var order = _orderCollection.AsQueryable()
                .FirstOrDefault(c => c.OrderId == id);

            if (order != null)
            {
                _orderCollection.DeleteOne(c => c.OrderId == id);
            }
        });
    }
}