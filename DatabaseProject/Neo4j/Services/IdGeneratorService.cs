using Neo4j.Driver;

namespace Database_project.Neo4j.Services
{
    public class IdGeneratorService
    {
        private readonly IDriver _driver;

        public IdGeneratorService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<long> GenerateNewIdAsync(string entityName)
        {
            var session = _driver.AsyncSession();
            try
            {
                var result = await session.ExecuteWriteAsync(async tx =>
                {
                    var query = $@"
                    MERGE (seq:Sequence {{name: '{entityName}Id'}})
                    ON CREATE SET seq.value = 1000
                    SET seq.value = seq.value + 1
                    RETURN seq.value AS newId";

                    var cursor = await tx.RunAsync(query);
                    var record = await cursor.SingleAsync();
                    return record["newId"].As<long>();
                });

                return result;
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}
