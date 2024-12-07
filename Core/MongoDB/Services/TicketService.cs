using Database_project.Core.MongoDB.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Database_project.Core.MongoDB.Services;

public class TicketService
{
    private readonly IMongoCollection<MongoDBTicket> _ticketCollection;
    private readonly OrderService _orderService;
    private readonly IMongoCollection<MongoDBOrder> _orderCollection;
    
    public TicketService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, OrderService orderService)
    {
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _ticketCollection = database.GetCollection<MongoDBTicket>("Ticket");
        _orderCollection = database.GetCollection<MongoDBOrder>("Order");
        _orderService = orderService;
    }
    
    public async Task<MongoDBTicket?> GetTicketByIdAsync(string id)
    {
        return await Task.Run(() =>
            _ticketCollection.AsQueryable()
                .FirstOrDefault(c => c.TicketId == id));
    }
    
    public async Task<List<MongoDBTicket>> GetTicketAsync()
    {
        return await Task.Run(() =>
            _ticketCollection.AsQueryable().ToList());
    }

    public async Task CreateTicketAsync(MongoDBTicket ticket)
    {
        var existingOrder = await _orderService.GetOrderByIdAsync(ticket.OrderId);
        bool firstTicketOfOrder = existingOrder == null;
        var order = existingOrder ?? new MongoDBOrder { OrderId = ObjectId.GenerateNewId().ToString() };
        
        ticket.SetOrder(order);
        
        await _ticketCollection.InsertOneAsync(ticket);
        
        order.TicketIds.Add(ticket.TicketId);
        
        if (firstTicketOfOrder) //new order
        {
            await _orderCollection.InsertOneAsync(order);
        }
        else //existing order
        {
            await _orderCollection.UpdateOneAsync(
                c => c.OrderId == order.OrderId,
                Builders<MongoDBOrder>.Update.Set(o => o.TicketIds, order.TicketIds)
            );
        }
    }

    public async Task UpdateTicketAsync(string id, MongoDBTicket updatedTicket)
    {
        var updateDefinition = Builders<MongoDBTicket>.Update
            .Set(c => c.TicketType, updatedTicket.TicketType)
            .Set(c => c.Price, updatedTicket.Price)
            .Set(c => c.CustomerId, updatedTicket.CustomerId)
            .Set(c => c.DepartureId, updatedTicket.DepartureId)
            .Set(c => c.LuggageIds, updatedTicket.LuggageIds);

        var result = await _ticketCollection.UpdateOneAsync(
            c => c.TicketId == id,
            updateDefinition);
        
        if (result.ModifiedCount == 0)
        {
            throw new Exception($"Ticket with ID {id} not found or no changes were made.");
        }
    }

    public async Task DeleteTicketAsync(string id)
    {
        var ticket = await _ticketCollection.AsQueryable()
            .FirstOrDefaultAsync(c => c.TicketId == id);

        if (ticket != null)
        {
            var order = await _orderService.GetOrderByIdAsync(ticket.OrderId);

            if (order != null)
            {
                await _orderCollection.UpdateOneAsync(
                    c => c.OrderId == order.OrderId,
                    Builders<MongoDBOrder>.Update.Pull(o => o.TicketIds, ticket.TicketId)
                );
            }
            
            await _ticketCollection.DeleteOneAsync(c => c.TicketId == id);
        }
    }
}