using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Database_project.Core.MongoDB.Entities;

namespace Database_project.Core.MongoDB.Services;

public class CustomerService
{
    private readonly IMongoCollection<MongoDBCustomer> _customersCollection;

    public CustomerService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _customersCollection = database.GetCollection<MongoDBCustomer>("Customers");
    }
    
    
    public async Task<MongoDBCustomer?> GetCustomerByIdAsync(string customerId)
    {
        return await Task.Run(() =>
            _customersCollection.AsQueryable()
                .FirstOrDefault(c => c.CustomerId == customerId));
    }

    public async Task<List<MongoDBCustomer>> GetCustomersAsync()
    {
        return await Task.Run(() =>
            _customersCollection.AsQueryable().ToList());
    }

    public async Task InsertCustomerAsync(MongoDBCustomer customer)
    {
        await _customersCollection.InsertOneAsync(customer);
    }

    public async Task UpdateCustomerAsync(string customerId, MongoDBCustomer updatedCustomer)
    {
        // Build the update definition for the fields you want to update
        var updateDefinition = Builders<MongoDBCustomer>.Update
            .Set(c => c.FirstName, updatedCustomer.FirstName)
            .Set(c => c.LastName, updatedCustomer.LastName)
            .Set(c => c.PassportNumber, updatedCustomer.PassportNumber);

        // Perform the update using the customerId filter
        var result = await _customersCollection.UpdateOneAsync(
            c => c.CustomerId == customerId, // Filter by customerId
            updateDefinition // Define the fields to update
        );

        if (result.ModifiedCount == 0)
        {
            // Optionally handle the case where no document was updated
            throw new Exception($"Customer with ID {customerId} not found or no changes were made.");
        }
    }

    public async Task DeleteCustomerAsync(string customerId)
    {
        await Task.Run(() =>
        {
            var customer = _customersCollection.AsQueryable()
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer != null)
            {
                _customersCollection.DeleteOne(c => c.CustomerId == customerId);
            }
        });
    }
}