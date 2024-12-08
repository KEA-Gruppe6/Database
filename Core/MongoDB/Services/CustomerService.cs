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
        ValidatePassportNumber(customer.PassportNumber);
        
        await _customersCollection.InsertOneAsync(customer);
    }

    public async Task UpdateCustomerAsync(string customerId, MongoDBCustomer updatedCustomer)
    {
        ValidatePassportNumber(updatedCustomer.PassportNumber);
        
        var updateDefinition = Builders<MongoDBCustomer>.Update
            .Set(c => c.FirstName, updatedCustomer.FirstName)
            .Set(c => c.LastName, updatedCustomer.LastName)
            .Set(c => c.PassportNumber, updatedCustomer.PassportNumber);

        
        var result = await _customersCollection.UpdateOneAsync(
            c => c.CustomerId == customerId, // Filter by customerId
            updateDefinition // Define the fields to update
        );

        if (result.ModifiedCount == 0)
        {
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
    
    public void ValidatePassportNumber(int passportNumber)
    {
        string passportString = passportNumber.ToString();
        if (passportString.Length != 9)
        {
            throw new InvalidOperationException("Passport number must be exactly 9 characters long.");
        }
    }
}