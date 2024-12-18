using Database_project.Neo4j.Entities;
using Neo4j.Driver;
using System.Diagnostics;
using System.Text.Json;

namespace Database_project.Neo4j.Services
{
    public class CustomerService
    {
        private readonly IDriver _driver;

        public CustomerService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<Customer?> GetCustomerByTicketIdAsync(long ticketId)
        {
            var (queryResults, _) = await _driver
                .ExecutableQuery(@"MATCH (t:Ticket)-[:ASSIGNED_TO]->(c:Customer)
                               WHERE t.TicketId = $ticketId
                               RETURN c.CustomerId AS CustomerId, 
                                      c.FirstName AS FirstName, 
                                      c.LastName AS LastName, 
                                      c.PassportNumber AS PassportNumber")
                .WithParameters(new { ticketId })
                .ExecuteAsync();

            return queryResults
                .Select(record => new Customer
                {
                    CustomerId = record["CustomerId"].As<long>(),
                    FirstName = record["FirstName"].As<string>(),
                    LastName = record["LastName"].As<string>(),
                    PassportNumber = record["PassportNumber"].As<int>()
                })
                .SingleOrDefault();
        }

        public async Task DeleteCustomerAsync(long customerId)
        {
            var session = _driver.AsyncSession();
            try
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    var deleteCustomerQuery = @"
                MATCH (c:Customer {CustomerId: $customerId})
                DETACH DELETE c";

                    var parameters = new { customerId };

                    // Log the query and parameters
                    Debug.WriteLine("Executing query: " + deleteCustomerQuery);
                    Debug.WriteLine("With parameters: " + JsonSerializer.Serialize(parameters));

                    var cursor = await tx.RunAsync(deleteCustomerQuery, parameters);
                    await cursor.ConsumeAsync();
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e.Message);
                throw new ArgumentException();
            }
            finally
            {
                await session.CloseAsync();
            }
        }

    }
}
