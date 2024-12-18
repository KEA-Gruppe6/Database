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
        private readonly IdGeneratorService _idGeneratorService;

        public TicketService(IDriver driver, CustomerService customerService, FlightrouteService flightrouteService, LuggageService luggageService, IdGeneratorService idGeneratorService)
        {
            _driver = driver;
            _customerService = customerService;
            _flightrouteService = flightrouteService;
            _luggageService = luggageService;
            _idGeneratorService = idGeneratorService;
        }

        public async Task<Ticket> CreateTicketAsync(Ticket ticket)
        {
            var session = _driver.AsyncSession();

            long newTicketId = 0L;

            try
            {
                newTicketId = await _idGeneratorService.GenerateNewIdAsync("Flightroute");


                await session.ExecuteWriteAsync(async tx =>
                {
                    var createTicketQuery = @"
                        MERGE (c:Customer {CustomerId: $customerId})
                        ON CREATE SET c.FirstName = $customerFirstName, c.LastName = $customerLastName, c.PassportNumber = $passportNumber
                        MERGE (f:Flightroute {FlightrouteId: $flightrouteId})
                        MERGE (tt:TicketType {TicketTypeId: $ticketTypeId})
                        MERGE (t:Ticket {TicketId: $ticketId})
                        ON CREATE SET t.Price = $price
                        MERGE (c)-[:PURCHASED]->(t)
                        MERGE (f)-[:HAS_TICKET]->(t)
                        MERGE (t)-[:HAS_TICKET_TYPE]->(tt)
                        WITH t
                        UNWIND $luggageList AS luggage
                        MERGE (l:Luggage {LuggageId: luggage.LuggageId})
                        ON CREATE SET l.Weight = luggage.Weight, l.IsCarryOn = luggage.IsCarryOn
                        MERGE (t)-[:HAS_LUGGAGE]->(l)";

                    var parameters = new
                    {
                        customerId = ticket.Customer.CustomerId,
                        customerFirstName = ticket.Customer.FirstName,
                        customerLastName = ticket.Customer.LastName,
                        passportNumber = ticket.Customer.PassportNumber,
                        flightrouteId = ticket.Flightroute.FlightrouteId,
                        ticketTypeId = ticket.TicketType.TicketTypeId,
                        ticketId = newTicketId,
                        price = ticket.Price,
                        luggageList = ticket.Luggage.Select(l => new
                        {
                            l.LuggageId,
                            l.Weight,
                            l.IsCarryOn
                        }).ToList()
                    };

                    // Log the query and parameters
                    Debug.WriteLine("Executing query: " + createTicketQuery);
                    Debug.WriteLine("With parameters: " + JsonSerializer.Serialize(parameters));

                    var cursor = await tx.RunAsync(createTicketQuery, parameters);
                    await cursor.ConsumeAsync();
                });

                return ticket;
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

        public async Task<Ticket?> GetTicketByIdAsync(long ticketId)
        {
            var (queryResults, _) = await _driver
                .ExecutableQuery(@"MATCH (t:Ticket)-[:HAS_TICKET_TYPE]->(tt:TicketType)
                           WHERE t.TicketId = $ticketId
                           RETURN t.TicketId AS TicketId, 
                                  t.Price AS Price, 
                                  t.TicketTypeId AS TicketTypeId,
                                  tt.TicketTypeName AS TicketTypeName")
                .WithParameters(new { ticketId })
                .ExecuteAsync();

            var record = queryResults.FirstOrDefault();
            if (record == null)
            {
                return null;
            }

            var ticket = new Ticket
            {
                TicketId = record["TicketId"].As<long>(),
                Price = record["Price"].As<double>(),
                TicketType = new TicketType
                {
                    TicketTypeId = record["TicketTypeId"].As<long>(),
                    TicketTypeName = record["TicketTypeName"].As<string>()
                },
            };

            ticket.Customer = await _customerService.GetCustomerByTicketIdAsync(ticket.TicketId);
            ticket.Flightroute = await _flightrouteService.GetFlightrouteByTicketIdAsync(ticket.TicketId);
            ticket.Luggage = await _luggageService.GetLuggageByTicketIdAsync(ticket.TicketId);

            return ticket;
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
