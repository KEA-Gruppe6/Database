using Database_project.Core.MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;

namespace Database_project.MongoDB_Query
{
    public class AddUsers
    {
        private readonly IMongoClient _mongoClient;
        private readonly MongoDbSettings _settings;

        public AddUsers(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            _mongoClient = mongoClient;
            _settings = settings.Value;
        }

        public void CreateMongoUsers()
        {
            var database = _mongoClient.GetDatabase(_settings.DatabaseName);

            if (!UserExists(database, "adminUser"))
            {
                CreateUser(database, "adminUser", "adminPassword", new[] {
                    new BsonDocument { { "role", "dbAdmin" }, { "db", "AirportDB" } },
                    new BsonDocument { { "role", "userAdmin" }, { "db", "AirportDB" } },
                    new BsonDocument { { "role", "readWrite" }, { "db", "AirportDB" } }
                });
            }

            if (!UserExists(database, "appUser"))
            {
                CreateUser(database, "appUser", "appPassword", new[] {
                    new BsonDocument { { "role", "readWrite" }, { "db", "AirportDB" } }
                });
            }

            if (!UserExists(database, "readOnlyUser"))
            {
                CreateUser(database, "readOnlyUser", "readonlyPassword", new[] {
                    new BsonDocument { { "role", "read" }, { "db", "AirportDB" } }
                });
            }
            /*
             * run this code for the role to be created
              db.createRole({
              role: "readInCustomers",  
              privileges: [
                {
                  resource: { db: "AirportDB", collection: "Customers" },  
                  actions: ["find"]
                }
              ],
              roles: []  
              });
             */
            if (!UserExists(database, "restrictedUser"))
            {
                CreateUser(database, "restrictedUser", "restrictedPassword", new[] {
                    new BsonDocument {
                        { "role", "readInCustomers" },
                        { "db", "AirportDB" }
                    }
                });
            }

        }

        private bool UserExists(IMongoDatabase database, string userName)
        {
            var users = database.GetCollection<BsonDocument>("system.users");
            var filter = Builders<BsonDocument>.Filter.Eq("user", userName);
            var user = users.Find(filter).FirstOrDefault();
            return user != null;
        }

        private void CreateUser(IMongoDatabase database, string userName, string password, BsonDocument[] roles)
        {
            var userCommand = new BsonDocument
            {
                { "createUser", userName },
                { "pwd", password },
                { "roles", new BsonArray(roles) }
            };

            try
            {
                database.RunCommand<BsonDocument>(userCommand);
                Console.WriteLine($"User '{userName}' created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user '{userName}': {ex.Message}");
            }
        }
    }
}