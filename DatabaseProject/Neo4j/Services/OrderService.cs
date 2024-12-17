using Database_project.Neo4j.Entities;
using Neo4j.Driver;
using System.Diagnostics;
using System.Text.Json;

namespace Database_project.Neo4j.Services
{
    public class OrderService
    {
        private readonly IDriver _driver;
        private readonly TicketService _ticketService;

        public OrderService(IDriver driver, TicketService ticketService)
        {
            _driver = driver;
            _ticketService = ticketService;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var (queryResults, _) = await _driver
                .ExecutableQuery(@"MATCH (o:Order)
                               RETURN o.OrderId AS OrderId, 
                                      o.AirlineConfirmationNumber AS AirlineConfirmationNumber")
                .ExecuteAsync();

            var orders = queryResults
                .Select(record => new Order
                {
                    OrderId = record["OrderId"].As<int>(),
                    AirlineConfirmationNumber = record["AirlineConfirmationNumber"].As<string>()
                })
                .ToList();

            foreach (var order in orders)
            {
                order.Tickets = await _ticketService.GetTicketsByOrderIdAsync(order.OrderId);
            }

            return orders;
        }

        public async Task<Order?> GetOrderByIdAsync(long id)
        {
            var (queryResults, _) = await _driver
                .ExecutableQuery(@"MATCH (o:Order)
                               WHERE o.OrderId = $id
                               RETURN o.OrderId AS OrderId, 
                                      o.AirlineConfirmationNumber AS AirlineConfirmationNumber")
                .WithParameters(new { id })
                .ExecuteAsync();

            var order = queryResults
                .Select(record => new Order
                {
                    OrderId = record["OrderId"].As<int>(),
                    AirlineConfirmationNumber = record["AirlineConfirmationNumber"].As<string>()
                })
                .SingleOrDefault();

            if (order != null)
            {
                order.Tickets = await _ticketService.GetTicketsByOrderIdAsync(order.OrderId);
            }

            return order;
        }

        public async Task DeleteOrderAsync(long orderId)
        {
            var session = _driver.AsyncSession();
            try
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    var deleteOrderQuery = @"
                MATCH (o:Order {OrderId: $orderId})
                DETACH DELETE o";

                    var parameters = new { orderId };

                    // Log the query and parameters
                    Debug.WriteLine("Executing query: " + deleteOrderQuery);
                    Debug.WriteLine("With parameters: " + JsonSerializer.Serialize(parameters));

                    var cursor = await tx.RunAsync(deleteOrderQuery, parameters);
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
