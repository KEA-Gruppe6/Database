using System.Data.SqlTypes;
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
        try
        {
            var customer = await context.Customers.FirstOrDefaultAsync(a => a.CustomerId == id) ?? null;
            return customer;
        }
        catch (SqlNullValueException)
        {
            return null;
        }


    }

    public async Task<Customer> CreateCustomerRawSQLAsync(Customer customer)
    {
        await using var context = await _context.CreateDbContextAsync();

        // Get the last customer ID to compare with the new one after the trigger is executed (manual tracking)
        // Uses raw SQL to access the Customers table instead of the view, CustomersDeletedIsNull, from EF Core
        var existingCustomer = await context.Customers
                .FromSqlRaw("SELECT TOP 1 * FROM Customers ORDER BY CustomerId DESC")
                .FirstOrDefaultAsync();
        var existingCustomerId = existingCustomer?.CustomerId ?? 0;

        // Use raw SQL to operate with trigger in the database. Uses parameters to prevent SQL injection.
        var sql = "INSERT INTO Customers (FirstName, LastName, PassportNumber) VALUES (@FirstName, @LastName, @PassportNumber)";
        var parameters = new[]
        {
        new SqlParameter("@FirstName", customer.FirstName),
        new SqlParameter("@LastName", customer.LastName),
        new SqlParameter("@PassportNumber", customer.PassportNumber),
    };
        await context.Database.ExecuteSqlRawAsync(sql, parameters);

        // Get the new customer to return it as EF Core won't automatically update the entity when executing Raw SQL (manual tracking)
        var returnCustomer = await context.Customers
            .OrderByDescending(c => c.CustomerId)
            .FirstOrDefaultAsync(c => c.FirstName == customer.FirstName && c.LastName == customer.LastName && c.PassportNumber == customer.PassportNumber);

        // Check if the new customer was created by comparing the last customer ID with the new one (manual tracking)
        if (existingCustomerId == returnCustomer.CustomerId)
        {
            throw new DbUpdateException("Customer was not created.");
        }

        return returnCustomer;
    }

    public async Task<Customer> CreateCustomerEFAddAsync(Customer customer)
    {
        await using var context = await _context.CreateDbContextAsync();

        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();

        // Reload the customer to ensure any changes made by the trigger are reflected
        await context.Entry(customer).ReloadAsync();

        return customer;
    }

    public async Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        await using var context = await _context.CreateDbContextAsync();

        // Use Raw SQL to prevent creating a new entity with nullable fields
        var existingCustomer = await context.Customers
            .FromSqlRaw("SELECT * FROM CustomersDeletedIsNull WHERE FirstName IS NOT NULL AND CustomerId = {0}", customer.CustomerId)
            .FirstOrDefaultAsync();

        if (existingCustomer == null)
        {
            throw new KeyNotFoundException("Customer not found.");
        }

        //Use raw SQL to operate with trigger in the database. Uses parameters to prevent SQL injection.
        var sql = "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, PassportNumber = @PassportNumber WHERE CustomerId = @CustomerId";
        var parameters = new[]
        {
            new SqlParameter("@FirstName", customer.FirstName),
            new SqlParameter("@LastName", customer.LastName),
            new SqlParameter("@PassportNumber", customer.PassportNumber),
            new SqlParameter("@CustomerId", customer.CustomerId)
        };
        await context.Database.ExecuteSqlRawAsync(sql, parameters);

        // Reload the customer to ensure any changes made by the trigger are reflected
        context.Entry(existingCustomer).State = EntityState.Detached;
        var updatedCustomer = await context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);
        if (existingCustomer == updatedCustomer)
        {
            throw new DbUpdateException("Customer was not updated.");
        }

        return updatedCustomer;
    }

    public async Task<Customer> DeleteCustomerAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();

        var customer = await context.Customers.FindAsync(id);
        if (customer == null)
        {
            throw new KeyNotFoundException("Customer not found.");
        }
        var returnEntityEntry = context.Customers.Remove(customer);
        await context.SaveChangesAsync();

        return returnEntityEntry.Entity;
    }

}