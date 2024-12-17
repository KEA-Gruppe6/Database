using Database_project.Neo4j.Entities;
using Neo4j.Driver;

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
    }
}
