using Database_project.Neo4j.Entities;
using Neo4j.Driver;
using System.Diagnostics;
using System.Text.Json;

namespace Database_project.Neo4j.Services
{
    public class LuggageService
    {
        private readonly IDriver _driver;

        public LuggageService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<List<Luggage>> GetLuggageByTicketIdAsync(long ticketId)
        {
            var (queryResults, _) = await _driver
                .ExecutableQuery(@"MATCH (t:Ticket)<-[:BELONGS_TO]-(l:Luggage)
                               WHERE t.TicketId = $ticketId
                               RETURN l.LuggageId AS LuggageId, 
                                      l.MaxWeight AS MaxWeight, 
                                      l.IsCarryOn AS IsCarryOn")
                .WithParameters(new { ticketId })
                .ExecuteAsync();

            return queryResults
                .Select(record => new Luggage
                {
                    LuggageId = record["LuggageId"].As<long>(),
                    Weight = record["MaxWeight"].As<double>(),
                    IsCarryOn = record["IsCarryOn"].As<bool>()
                })
                .ToList();
        }

        public async Task DeleteLuggageAsync(long luggageId)
        {
            var session = _driver.AsyncSession();
            try
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    var deleteLuggageQuery = @"
                MATCH (l:Luggage {LuggageId: $luggageId})
                DETACH DELETE l";

                    var parameters = new { luggageId };

                    // Log the query and parameters
                    Debug.WriteLine("Executing query: " + deleteLuggageQuery);
                    Debug.WriteLine("With parameters: " + JsonSerializer.Serialize(parameters));

                    var cursor = await tx.RunAsync(deleteLuggageQuery, parameters);
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
