using Database_project.Neo4j.Entities;
using Neo4j.Driver;
using System.Diagnostics;
using System.Text.Json;

namespace Database_project.Neo4j.Services
{
    public class FlightrouteService
    {
        private readonly IDriver _driver;
        private readonly PlaneService _planeService;
        private readonly AirportService _airportService;
        private readonly IdGeneratorService _idGeneratorService;

        public FlightrouteService(IDriver driver, PlaneService planeService, AirportService airportService, IdGeneratorService idGeneratorService)
        {
            _driver = driver;
            _planeService = planeService;
            _airportService = airportService;
            _idGeneratorService = idGeneratorService;
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

        public async Task<Flightroute?> CreateFlightrouteAsync(Flightroute flightroute)
        {
            var session = _driver.AsyncSession();
            var newFlightrouteId = 0L;
            try
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    // Generate a new FlightrouteId using the IdGeneratorService
                    newFlightrouteId = await _idGeneratorService.GenerateNewIdAsync("Flightroute");

                    var createFlightrouteQuery = @"
                CREATE (fr:Flightroute {FlightrouteId: $FlightrouteId, DepartureTime: $DepartureTime, ArrivalTime: $ArrivalTime})
                WITH fr
                MERGE (p:Plane {PlaneId: $PlaneId})
                ON CREATE SET p.PlaneDisplayName = $PlaneDisplayName
                CREATE (fr)-[:USES_PLANE]->(p)
                WITH fr
                MERGE (da:Airport {AirportId: $DepartureAirportId})
                ON CREATE SET da.AirportName = $DepartureAirportName, da.AirportCity = $DepartureAirportCity, da.Municipality = $DepartureMunicipality, da.AirportAbbreviation = $DepartureAirportAbbreviation
                CREATE (fr)-[:DEPARTS_FROM]->(da)
                WITH fr
                MERGE (aa:Airport {AirportId: $ArrivalAirportId})
                ON CREATE SET aa.AirportName = $ArrivalAirportName, aa.AirportCity = $ArrivalAirportCity, aa.Municipality = $ArrivalMunicipality, aa.AirportAbbreviation = $ArrivalAirportAbbreviation
                CREATE (fr)-[:ARRIVES_AT]->(aa)
                RETURN fr";

                    var parameters = new
                    {
                        FlightrouteId = newFlightrouteId,
                        flightroute.DepartureTime,
                        flightroute.ArrivalTime,
                        flightroute.Plane.PlaneId,
                        flightroute.Plane.PlaneDisplayName,
                        DepartureAirportId = flightroute.DepartureAirport.AirportId,
                        DepartureAirportName = flightroute.DepartureAirport.AirportName,
                        DepartureAirportCity = flightroute.DepartureAirport.AirportCity,
                        DepartureMunicipality = flightroute.DepartureAirport.Municipality,
                        DepartureAirportAbbreviation = flightroute.DepartureAirport.AirportAbbreviation,
                        ArrivalAirportId = flightroute.ArrivalAirport.AirportId,
                        ArrivalAirportName = flightroute.ArrivalAirport.AirportName,
                        ArrivalAirportCity = flightroute.ArrivalAirport.AirportCity,
                        ArrivalMunicipality = flightroute.ArrivalAirport.Municipality,
                        ArrivalAirportAbbreviation = flightroute.ArrivalAirport.AirportAbbreviation
                    };

                    // Log the query and parameters
                    Debug.WriteLine("Executing query: " + createFlightrouteQuery);
                    Debug.WriteLine("With parameters: " + JsonSerializer.Serialize(parameters));

                    var cursor = await tx.RunAsync(createFlightrouteQuery, parameters);
                    await cursor.ConsumeAsync();

                });

                return await GetFlightrouteByFlightrouteIdAsync(newFlightrouteId);
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

        public async Task<Flightroute?> GetFlightrouteByFlightrouteIdAsync(long flightrouteId)
        {
            var (queryResults, _) = await _driver
                .ExecutableQuery(@"MATCH (fr:Flightroute {FlightrouteId: $flightrouteId})
                               RETURN fr.FlightrouteId AS FlightrouteId, 
                                      fr.DepartureTime AS DepartureTime, 
                                      fr.ArrivalTime AS ArrivalTime")
                .WithParameters(new { flightrouteId })
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

        public async Task DeleteFlightrouteAsync(long flightrouteId)
        {
            await _driver.ExecutableQuery(@"MATCH (fr:Flightroute {FlightrouteId: $flightrouteId})
                                        DETACH DELETE fr")
                .WithParameters(new { flightrouteId })
                .ExecuteAsync();
        }
    }
}
