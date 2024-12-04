using Database_project.Core;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Services;

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

        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();

        return customer;
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