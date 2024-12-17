using Database_project.Neo4j.Entities;
using Neo4j.Driver;
using System.Diagnostics;
using System.Text.Json;

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

        public async Task DeletePlaneAsync(long planeId)
        {
            var session = _driver.AsyncSession();
            try
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    var deletePlaneQuery = @"
                MATCH (p:Plane {PlaneId: $planeId})
                DETACH DELETE p";

                    var parameters = new { planeId };

                    // Log the query and parameters
                    Debug.WriteLine("Executing query: " + deletePlaneQuery);
                    Debug.WriteLine("With parameters: " + JsonSerializer.Serialize(parameters));

                    var cursor = await tx.RunAsync(deletePlaneQuery, parameters);
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
