using Database_project.Core.Entities;
using Neo4j.Driver;

namespace Database_project.Neo4j.Services
{
    public class OrderService
    {
        private readonly IDriver _driver;
        public OrderService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var (queryResults, _) = await _driver
            .ExecutableQuery(@"MATCH (o:Order)-[:CONTAINS]->(t:Ticket)
                    RETURN o.OrderId AS OrderId, o.AirlineConfirmationNumber AS AirlineConfirmationNumber, 
                           collect({TicketId: t.TicketId, Price: t.Price}) AS Tickets")
            .ExecuteAsync();

            return queryResults
                .Select(record => new Order
                {
                    OrderId = record["OrderId"].As<int>(),
                    AirlineConfirmationNumber = record["AirlineConfirmationNumber"].As<string>(),
                    Tickets = record["Tickets"]
                        .As<List<IDictionary<string, object>>>()
                        .Select(ticket => new Ticket
                        {
                            TicketId = (long)ticket["TicketId"],
                            Price = (double)ticket["Price"]
                        })
                        .ToList()
                })
                .ToList();
        }

        public async Task<Order?> GetOrderByIdAsync(long id)
        {
            var (queryResults, _) = await _driver
            .ExecutableQuery(@"MATCH (o:Order)-[:CONTAINS]->(t:Ticket)
                    WHERE o.OrderId = $id
                    RETURN o.OrderId AS OrderId, o.AirlineConfirmationNumber AS AirlineConfirmationNumber, 
                           collect({TicketId: t.TicketId, Price: t.Price}) AS Tickets")
            .WithParameters(new { id })
            .ExecuteAsync();

            return queryResults
                .Select(record => new Order
                {
                    OrderId = record["OrderId"].As<int>(),
                    AirlineConfirmationNumber = record["AirlineConfirmationNumber"].As<string>(),
                    Tickets = record["Tickets"]
                        .As<List<IDictionary<string, object>>>()
                        .Select(ticket => new Ticket
                        {
                            TicketId = (long)ticket["TicketId"],
                            Price = (double)ticket["Price"]
                        })
                        .ToList()
                })
                .SingleOrDefault();
        }

    }
}
