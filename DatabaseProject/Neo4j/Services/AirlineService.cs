using Database_project.Neo4j.Entities;
using Neo4j.Driver;


namespace Database_project.Neo4j.Services
{
    public class AirlineService
    {
        private readonly IDriver _driver;
        public AirlineService(IDriver driver)
        {
            _driver = driver;
        }
        public async Task<Airline?> GetAirlineByIdAsync(long id)
        {
            var (queryResults, _) = await _driver
            .ExecutableQuery(@"MATCH (a:Airline)-[:OWNS]->(p:Plane)
                    WHERE a.AirlineId = $id
                    RETURN a.AirlineId AS AirlineId, a.AirlineName AS AirlineName, 
                           collect({PlaneId: p.PlaneId, PlaneDisplayName: p.PlaneDisplayName}) AS Planes")
            .WithParameters(new { id })
            .ExecuteAsync();

            return queryResults
                .Select(record => new Airline
                {
                    AirlineId = record["AirlineId"].As<long>(),
                    AirlineName = record["AirlineName"].As<string>(),
                    Planes = record["Planes"]
                        .As<List<IDictionary<string, object>>>()
                        .Select(plane => new Plane
                        {
                            PlaneId = (long)plane["PlaneId"],
                            PlaneDisplayName = (string)plane["PlaneDisplayName"]
                        })
                        .ToList()
                })
                .SingleOrDefault();
        }

        public async Task<List<Airline>> GetAllAirlinesAsync()
        {
            var (queryResults, _) = await _driver
                .ExecutableQuery(@"
                    MATCH (a:Airline)<-[:BELONGS_TO]-(p:Plane)
                    RETURN a.AirlineId AS AirlineId, a.AirlineName AS AirlineName, 
                           collect({PlaneId: p.PlaneId, PlaneDisplayName: p.PlaneDisplayName}) AS Planes")
                .ExecuteAsync();

            return queryResults
                .Select(record => new Airline
                {
                    AirlineId = record["AirlineId"].As<long>(),
                    AirlineName = record["AirlineName"].As<string>(),
                    Planes = record["Planes"]
                        .As<List<IDictionary<string, object>>>()
                        .Select(plane => new Plane
                        {
                            PlaneId = (long)plane["PlaneId"],
                            PlaneDisplayName = (string)plane["PlaneDisplayName"]
                        })
                        .ToList()
                })
                .ToList();
        }

        public async Task AddAirlineAsync(Airline airline)
        {
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    var createAirlineQuery = @"
                        CREATE (a:Airline {AirlineId: $AirlineId, AirlineName: $AirlineName})";

                    await tx.RunAsync(createAirlineQuery, new { airline.AirlineId, airline.AirlineName });

                    foreach (var plane in airline.Planes)
                    {
                        var createPlaneQuery = @"
                            CREATE (p:Plane {PlaneId: $PlaneId, PlaneDisplayName: $PlaneDisplayName})
                            WITH p
                            MATCH (a:Airline {AirlineId: $AirlineId})
                            CREATE (a)<-[:BELONGS_TO]-(p)";

                        await tx.RunAsync(createPlaneQuery, new { plane.PlaneId, plane.PlaneDisplayName, airline.AirlineId });
                    }
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task DeleteAirlineAsync(long id)
        {
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    var deleteRelationshipsQuery = @"
                        MATCH (a:Airline)<-[:BELONGS_TO]-(p:Plane)
                        WHERE a.AirlineId = $id
                        DELETE r";

                    await tx.RunAsync(deleteRelationshipsQuery, new { id });

                    var deleteAirlineQuery = @"
                        MATCH (a:Airline)
                        WHERE a.AirlineId = $id
                        DELETE a";

                    await tx.RunAsync(deleteAirlineQuery, new { id });
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }


        public async Task UpdateAirlineAsync(long id, Airline updatedAirline)
        {
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    var updateAirlineQuery = @"
                MATCH (a:Airline)
                WHERE a.AirlineId = $id
                SET a.AirlineName = $AirlineName";

                    await tx.RunAsync(updateAirlineQuery, new { id, updatedAirline.AirlineName });

                    var deleteRelationshipsQuery = @"
                MATCH (a:Airline)<-[:BELONGS_TO]-(p:Plane)
                WHERE a.AirlineId = $id
                DELETE r";

                    await tx.RunAsync(deleteRelationshipsQuery, new { id });

                    foreach (var plane in updatedAirline.Planes)
                    {
                        var createPlaneQuery = @"
                    MERGE (p:Plane {PlaneId: $PlaneId, PlaneDisplayName: $PlaneDisplayName})
                    WITH p
                    MATCH (a:Airline {AirlineId: $id})
                    MERGE (a)<-[:BELONGS_TO]-(p)";

                        await tx.RunAsync(createPlaneQuery, new { plane.PlaneId, plane.PlaneDisplayName, id });
                    }
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }

    }
}
