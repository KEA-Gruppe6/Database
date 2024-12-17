using Database_project.Neo4j.Entities;
using Neo4j.Driver;

namespace Database_project.Neo4j.Services
{
    public class FlightrouteService
    {
        private readonly IDriver _driver;
        private readonly PlaneService _planeService;
        private readonly AirportService _airportService;

        public FlightrouteService(IDriver driver, PlaneService planeService, AirportService airportService)
        {
            _driver = driver;
            _planeService = planeService;
            _airportService = airportService;
        }

        public async Task<Flightroute?> GetFlightrouteByTicketIdAsync(long ticketId)
        {
            var (queryResults, _) = await _driver
                .ExecutableQuery(@"MATCH (t:Ticket)-[:FOR_FLIGHTROUTE]->(fr:Flightroute)
                               WHERE t.TicketId = $ticketId
                               RETURN fr.FlightrouteId AS FlightrouteId, 
                                      fr.DepartureTime AS DepartureTime, 
                                      fr.ArrivalTime AS ArrivalTime")
                .WithParameters(new { ticketId })
                .ExecuteAsync();

            var flightroute = queryResults
                .Select(record => new Flightroute
                {
                    FlightrouteId = record["FlightrouteId"].As<long>(),
                    DepartureTime = DateTime.Parse(record["DepartureTime"].As<string>()),
                    ArrivalTime = DateTime.Parse(record["ArrivalTime"].As<string>())
                })
                .SingleOrDefault();

            if (flightroute != null)
            {
                flightroute.Plane = await _planeService.GetPlaneByFlightrouteIdAsync(flightroute.FlightrouteId);
                var airports = await _airportService.GetAirportsForFlightrouteAsync(flightroute.FlightrouteId);
                flightroute.DepartureAirport = airports.FirstOrDefault();
                flightroute.ArrivalAirport = airports.LastOrDefault();
            }

            return flightroute;
        }
    }
}
