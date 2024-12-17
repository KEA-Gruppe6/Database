using Database_project.Neo4j.Entities;
using Neo4j.Driver;

namespace Database_project.Neo4j.Services
{
    public class AirportService
    {
        private readonly IDriver _driver;

        public AirportService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<List<Airport>> GetAirportsForFlightrouteAsync(long flightrouteId)
        {
            var (queryResults, _) = await _driver
                .ExecutableQuery(@"MATCH (fr:Flightroute)-[:DEPARTS_FROM]->(da:Airport), 
                                          (fr)-[:ARRIVES_AT]->(aa:Airport)
                                   WHERE fr.FlightrouteId = $flightrouteId
                                   RETURN da.AirportId AS DepartureAirportId, 
                                          da.AirportName AS DepartureAirportName, 
                                          da.AirportCity AS DepartureAirportCity, 
                                          da.Municipality AS DepartureMunicipality, 
                                          da.AirportAbbreviation AS DepartureAirportAbbreviation,
                                          aa.AirportId AS ArrivalAirportId, 
                                          aa.AirportName AS ArrivalAirportName, 
                                          aa.AirportCity AS ArrivalAirportCity, 
                                          aa.Municipality AS ArrivalMunicipality, 
                                          aa.AirportAbbreviation AS ArrivalAirportAbbreviation")
                .WithParameters(new { flightrouteId })
                .ExecuteAsync();

            var airports = queryResults
                .Select(record => new Airport
                {
                    AirportId = record["DepartureAirportId"].As<long>(),
                    AirportName = record["DepartureAirportName"].As<string>(),
                    AirportCity = record["DepartureAirportCity"].As<string>(),
                    Municipality = record["DepartureMunicipality"].As<string>(),
                    AirportAbbreviation = record["DepartureAirportAbbreviation"].As<string>()
                })
                .Union(queryResults
                .Select(record => new Airport
                {
                    AirportId = record["ArrivalAirportId"].As<long>(),
                    AirportName = record["ArrivalAirportName"].As<string>(),
                    AirportCity = record["ArrivalAirportCity"].As<string>(),
                    Municipality = record["ArrivalMunicipality"].As<string>(),
                    AirportAbbreviation = record["ArrivalAirportAbbreviation"].As<string>()
                }))
                .ToList();

            return airports;
        }
    }
}
