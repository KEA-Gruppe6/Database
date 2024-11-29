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

    public async Task<bool> UpdateCustomerAsync(Customer customer)
    {
        await using var context = await _context.CreateDbContextAsync();
        try
        {
            var existingCustomer = await context.Customers
                .FirstOrDefaultAsync(a => a.CustomerId == customer.CustomerId);

            if (existingCustomer == null)
            {
                return false;  // If the customer doesn't exist, return false
            }

            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.PassportNumber = customer.PassportNumber;

            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> DeleteCustomerAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();
        try
        {
            var customer = await context.Customers.FindAsync(id);
            if (customer == null)
            {
                return false;
            }

            context.Customers.Remove(customer);
            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}