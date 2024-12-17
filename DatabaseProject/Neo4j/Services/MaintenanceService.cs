using Neo4j.Driver;
using Database_project.Neo4j.Entities;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;


namespace Database_project.Neo4j.Services
{
    public class MaintenanceService
    {
        private readonly IDriver _driver;

        public MaintenanceService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<List<Maintenance>> GetMaintenanceByPlaneIdAsync(long planeId)
        {
            var (queryResults, _) = await _driver
                .ExecutableQuery(@"MATCH (p:Plane)-[:HAS_MAINTENANCE]->(m:Maintenance)
                                   WHERE p.PlaneId = $planeId
                                   RETURN m.MaintenanceId AS MaintenanceId, 
                                          m.StartDate AS StartDate, 
                                          m.EndDate AS EndDate, 
                                          m.AirportId AS AirportId, 
                                          m.PlaneId AS PlaneId")
                .WithParameters(new { planeId })
                .ExecuteAsync();

            return queryResults
                .Select(record => new Maintenance
                {
                    MaintenanceId = record["MaintenanceId"].As<long>(),
                    StartDate = record["StartDate"].As<DateTime>(),
                    EndDate = record["EndDate"].As<DateTime>()
                })
                .ToList();
        }

        public async Task DeleteMaintenanceAsync(long maintenanceId)
        {
            var session = _driver.AsyncSession();
            try
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    var deleteMaintenanceQuery = @"
                        MATCH (m:Maintenance {MaintenanceId: $maintenanceId})
                        DETACH DELETE m";

                    var parameters = new { maintenanceId };

                    // Log the query and parameters
                    Debug.WriteLine("Executing query: " + deleteMaintenanceQuery);
                    Debug.WriteLine("With parameters: " + JsonSerializer.Serialize(parameters));

                    var cursor = await tx.RunAsync(deleteMaintenanceQuery, parameters);
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
