using Database_project.Neo4j.Entities;
using Neo4j.Driver;

namespace Database_project.Neo4j.Services
{
    public class PlaneService
    {
        private readonly IDriver _driver;

        public PlaneService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<Plane?> GetPlaneByFlightrouteIdAsync(long flightrouteId)
        {
            var (queryResults, _) = await _driver
                .ExecutableQuery(@"MATCH (fr:Flightroute)-[:USES_PLANE]->(p:Plane)
                                   WHERE fr.FlightrouteId = $flightrouteId
                                   RETURN p.PlaneId AS PlaneId, 
                                          p.PlaneDisplayName AS PlaneDisplayName, 
                                          p.AirlineId AS AirlineId")
                .WithParameters(new { flightrouteId })
                .ExecuteAsync();

            return queryResults
                .Select(record => new Plane
                {
                    PlaneId = record["PlaneId"].As<long>(),
                    PlaneDisplayName = record["PlaneDisplayName"].As<string>()
                })
                .SingleOrDefault();
        }
    }
}
