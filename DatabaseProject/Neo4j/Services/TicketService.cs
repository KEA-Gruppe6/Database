using Database_project.Neo4j.Entities;
using Neo4j.Driver;
using System.Diagnostics;
using System.Text.Json;

namespace Database_project.Neo4j.Services
{
    public class TicketService
    {
        private readonly IDriver _driver;
        private readonly CustomerService _customerService;
        private readonly FlightrouteService _flightrouteService;
        private readonly LuggageService _luggageService;

        public TicketService(IDriver driver, CustomerService customerService, FlightrouteService flightrouteService, LuggageService luggageService)
        {
            _driver = driver;
            _customerService = customerService;
            _flightrouteService = flightrouteService;
            _luggageService = luggageService;
        }

        public async Task<List<Ticket>> GetTicketsByOrderIdAsync(long orderId)
        {
            var (queryResults, _) = await _driver
                .ExecutableQuery(@"MATCH (o:Order)-[:CONTAINS]->(t:Ticket)-[:HAS_TICKET_TYPE]->(tt:TicketType)
                                   WHERE o.OrderId = $orderId
                                   RETURN t.TicketId AS TicketId, 
                                          t.Price AS Price, 
                                          t.TicketTypeId AS TicketTypeId,
                                          tt.TicketTypeName AS TicketTypeName")
                .WithParameters(new { orderId })
                .ExecuteAsync();

            var tickets = queryResults
                .Select(record => new Ticket
                {
                    TicketId = record["TicketId"].As<long>(),
                    Price = record["Price"].As<double>(),
                    TicketType = new TicketType
                    {
                        TicketTypeId = record["TicketTypeId"].As<long>(),
                        TicketTypeName = record["TicketTypeName"].As<string>()
                    },
                })
                .ToList();

            foreach (var ticket in tickets)
            {
                ticket.Customer = await _customerService.GetCustomerByTicketIdAsync(ticket.TicketId);
                ticket.Flightroute = await _flightrouteService.GetFlightrouteByTicketIdAsync(ticket.TicketId);
                ticket.Luggage = await _luggageService.GetLuggageByTicketIdAsync(ticket.TicketId);
            }

            return tickets;
        }


        public async Task DeleteTicketAsync(long ticketId)
        {
            var session = _driver.AsyncSession();
            try
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    var deleteTicketQuery = @"
                MATCH (t:Ticket {TicketId: $ticketId})
                DETACH DELETE t";

                    var parameters = new { ticketId };

                    // Log the query and parameters
                    Debug.WriteLine("Executing query: " + deleteTicketQuery);
                    Debug.WriteLine("With parameters: " + JsonSerializer.Serialize(parameters));

                    var cursor = await tx.RunAsync(deleteTicketQuery, parameters);
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
