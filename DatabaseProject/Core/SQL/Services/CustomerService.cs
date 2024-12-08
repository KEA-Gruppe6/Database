using Database_project.Core.SQL;
using Database_project.Core.SQL.Entities;
using Database_project.Core.SQL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core.Services;

public class CustomerService : ICustomerService
{
    private readonly IDbContextFactory<DatabaseContext> _context;

    public CustomerService(IDbContextFactory<DatabaseContext> context)
    {
        _context = context;
    }

    public async Task<Customer?> GetCustomerByIdAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();
        var customer = await context.Customers.FirstOrDefaultAsync(a => a.CustomerId == id);

        return customer;
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        await using var context = await _context.CreateDbContextAsync();

        var existingCustomer = await context.Customers.FindAsync(customer.CustomerId);
        if (existingCustomer != null)
        {
            throw new ArgumentException($"Customer with ID {customer.CustomerId} already exists.");
        }

        var sql = "INSERT INTO Customers (FirstName, LastName, PassportNumber) VALUES (@FirstName, @LastName, @PassportNumber)";
        var parameters = new[]
        {
                new SqlParameter("@FirstName", customer.FirstName),
                new SqlParameter("@LastName", customer.LastName),
                new SqlParameter("@PassportNumber", customer.PassportNumber),
            };
        await context.Database.ExecuteSqlRawAsync(sql, parameters);

        return context.Customers.OrderByDescending(c => c.CustomerId).FirstOrDefault();
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        await using var context = await _context.CreateDbContextAsync();

        // Retrieve the existing customer from the database
        var existingCustomer = await context.Customers.FindAsync(customer.CustomerId);
        if (existingCustomer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {customer.CustomerId} not found.");
        }

        existingCustomer.FirstName = customer.FirstName;
        existingCustomer.LastName = customer.LastName;
        existingCustomer.PassportNumber = customer.PassportNumber;

        var returnEntityEntry = context.Customers.Update(existingCustomer);
        await context.SaveChangesAsync();

        return returnEntityEntry.Entity;
    }

    public async Task<Customer> DeleteCustomerAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();

        var customer = await context.Customers.FindAsync(id);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found.");
        }
        var returnEntityEntry = context.Customers.Remove(customer);
        await context.SaveChangesAsync();

        return returnEntityEntry.Entity;
    }

}