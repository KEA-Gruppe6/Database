using Database_project.Core.SQL.DTOs;
using Database_project.Core.SQL.Entities;

namespace Database_project.Core.SQL.Interfaces;

public interface ICustomerService
{
    Task<Customer?> GetCustomerByIdAsync(long id);
    Task<Customer> CreateCustomerEFAddAsync(Customer customer);
    Task<Customer> CreateCustomerRawSQLAsync(Customer customer);
    Task<Customer> UpdateCustomerAsync(Customer customer);
    Task<Customer> DeleteCustomerAsync(long id);
}