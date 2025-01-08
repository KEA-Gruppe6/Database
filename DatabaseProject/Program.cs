using Database_project.Core.SQL;
using Database_project.Core.MongoDB;
using Database_project.Core.SQL.Interfaces;
using Database_project.MongoDB_Query;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Neo4j.Driver;


var builder = WebApplication.CreateBuilder(args);

//Get the connection string from the appsettings.json file or from the environment variable
string connectionString;
string neo4jUrl;
string neo4jUser;
string neo4jPassword;
string mongoDbConnectionString;
string mongoDbDatabaseName;
if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetValue<string>("MSSQL") ?? throw new ArgumentNullException();
    neo4jUrl = builder.Configuration.GetValue<string>("Neo4jUrl") ?? throw new ArgumentNullException();
    neo4jUser = builder.Configuration.GetValue<string>("Neo4jUser") ?? throw new ArgumentNullException();
    neo4jPassword = builder.Configuration.GetValue<string>("Neo4jPassword") ?? throw new ArgumentNullException();
    mongoDbConnectionString = builder.Configuration.GetValue<string>("MongoDB") ?? throw new ArgumentNullException();
    mongoDbDatabaseName = builder.Configuration.GetValue<string>("DatabaseName") ?? throw new ArgumentNullException();
}
else
{
    connectionString = Environment.GetEnvironmentVariable("MSSQL") ?? throw new ArgumentNullException();
    neo4jUrl = Environment.GetEnvironmentVariable("Neo4jUrl") ?? throw new ArgumentNullException();
    neo4jUser = Environment.GetEnvironmentVariable("Neo4jUser") ?? throw new ArgumentNullException();
    neo4jPassword = Environment.GetEnvironmentVariable("Neo4jPassword") ?? throw new ArgumentNullException();
    mongoDbConnectionString = Environment.GetEnvironmentVariable("MongoDB") ?? throw new ArgumentNullException();
    mongoDbDatabaseName = Environment.GetEnvironmentVariable("DatabaseName") ?? throw new ArgumentNullException();
}

// Add Database context
builder.Services.AddDbContextFactory<DatabaseContext>(options =>
{
    options.UseSqlServer(connectionString);
});

//TODO: Authentication of API calls
// Setup for MongoDb driver
builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionString = mongoDbConnectionString;
    options.DatabaseName = mongoDbDatabaseName;
});

// Singleton pattern implementation for connection
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Add Neo4j driver
builder.Services.AddSingleton(GraphDatabase.Driver(neo4jUrl, AuthTokens.Basic(neo4jUser, neo4jPassword)));

builder.Services.AddControllers();
// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("mssql", new OpenApiInfo { Title = "Database project MSSQL", Version = "mssql" });

    c.SwaggerDoc("mongodb", new OpenApiInfo { Title = "Database project MongoDB", Version = "mongodb" });

    c.SwaggerDoc("neo4j", new OpenApiInfo { Title = "Database project Neo4j", Version = "neo4j" });

    // Group by route template
    c.DocInclusionPredicate((name, api) =>
    {
        var routeTemplate = (api.ActionDescriptor as ControllerActionDescriptor)?.AttributeRouteInfo?.Template;
        if (name == "mssql")
        {
            return routeTemplate != null && routeTemplate.StartsWith("api/mssql");
        }
        if (name == "mongodb")
        {
            return routeTemplate != null && routeTemplate.StartsWith("api/MongoDB");
        }
        if (name == "neo4j")
        {
            return routeTemplate != null && routeTemplate.StartsWith("api/neo4j");
        }
        return false;
    });

    // Define the grouping mechanism
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
        if (controllerActionDescriptor != null)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        return new[] { "Default" };
    });
});

builder.Services.AddScoped<ICustomerService, Database_project.Core.Services.CustomerService>();
builder.Services.AddScoped<IAirlineService, Database_project.Core.Services.AirlineService>();
builder.Services.AddScoped<IAirportService, Database_project.Core.Services.AirportService>();
builder.Services.AddScoped<ITicketTypeService, Database_project.Core.Services.TicketTypeService>();
builder.Services.AddScoped<IPlaneService, Database_project.Core.Services.PlaneService>();
builder.Services.AddScoped<IOrderService, Database_project.Core.Services.OrderService>();
builder.Services.AddScoped<IFlightrouteService, Database_project.Core.Services.FlightrouteService>();
builder.Services.AddScoped<ILuggageService, Database_project.Core.Services.LuggageService>();
builder.Services.AddScoped<IMaintenanceService, Database_project.Core.Services.MaintenanceService>();
builder.Services.AddScoped<ITicketService, Database_project.Core.Services.TicketService>();

// Register MongoDB services
builder.Services.AddScoped<Database_project.Core.MongoDB.Services.CustomerService>();
builder.Services.AddScoped<Database_project.Core.MongoDB.Services.AirlineService>();
builder.Services.AddScoped<Database_project.Core.MongoDB.Services.AirportService>();
builder.Services.AddScoped<Database_project.Core.MongoDB.Services.OrderService>();
builder.Services.AddScoped<Database_project.Core.MongoDB.Services.TicketService>();
builder.Services.AddScoped<Database_project.Core.MongoDB.Services.FlightrouteService>();
builder.Services.AddScoped<Database_project.Core.MongoDB.Services.LuggageService>();
builder.Services.AddScoped<Database_project.Core.MongoDB.Services.PlaneService>();
builder.Services.AddScoped<Database_project.Core.MongoDB.Services.MaintenanceService>();
builder.Services.AddScoped<MongoDBSeeder>();

// Register Neo4j services
builder.Services.AddScoped<Database_project.Neo4j.Services.IdGeneratorService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.AirlineService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.OrderService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.TicketService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.CustomerService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.LuggageService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.FlightrouteService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.AirportService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.PlaneService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.MaintenanceService>();

// Register AddUsers as scoped service
builder.Services.AddScoped<AddUsers>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // SQL Server migration starts
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    var retryCount = 5;
    var delay = TimeSpan.FromSeconds(10);

    for (int i = 0; i < retryCount; i++)
    {
        try
        {
            dbContext.Database.Migrate();
            break;
        }
        catch (SqlException)
        {
            if (i == retryCount - 1) throw;
            Thread.Sleep(delay);
        }
    }

    // Add passport length trigger    
    var sqlFilePath = "SQL_Query/passportlengthtrigger.sql";
    var sqlQuery = File.ReadAllText(sqlFilePath);
    dbContext.Database.ExecuteSqlRaw("IF OBJECT_ID('trg_ValidatePassportNumber', 'TR') IS NOT NULL DROP TRIGGER trg_ValidatePassportNumber;");
    dbContext.Database.ExecuteSqlRaw(sqlQuery);

    // Add customer audit trigger
    sqlFilePath = "SQL_Query/customeraudittrigger.sql";
    sqlQuery = File.ReadAllText(sqlFilePath);
    dbContext.Database.ExecuteSqlRaw("IF OBJECT_ID('trg_AuditCustomerInsert', 'TR') IS NOT NULL DROP TRIGGER trg_AuditCustomerInsert;");
    dbContext.Database.ExecuteSqlRaw("IF OBJECT_ID('trg_AuditCustomerUpdate', 'TR') IS NOT NULL DROP TRIGGER trg_AuditCustomerUpdate;");
    dbContext.Database.ExecuteSqlRaw("IF OBJECT_ID('trg_AuditCustomerDelete', 'TR') IS NOT NULL DROP TRIGGER trg_AuditCustomerDelete;");
    var sqlCommands = sqlQuery.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

    foreach (var command in sqlCommands)
    {
        dbContext.Database.ExecuteSqlRaw(command);
    }

    // Run seed SQL query
    sqlFilePath = "SQL_Query/populatedb.sql";
    sqlQuery = File.ReadAllText(sqlFilePath);
    dbContext.Database.ExecuteSqlRaw(sqlQuery);

    // Add specific roles
    sqlFilePath = "SQL_Query/addroles.sql";
    sqlQuery = File.ReadAllText(sqlFilePath);
    dbContext.Database.ExecuteSqlRaw(sqlQuery);
    // SQL Server migration ends

    // MongoDB migration starts
    if (mongoDbConnectionString.Contains("localhost") || mongoDbConnectionString.Contains("host.docker.internal"))
    {
        var addUsersService = scope.ServiceProvider.GetRequiredService<AddUsers>();
        addUsersService.CreateMongoUsers();  // Run the user creation script
    }
    var mongodSeeder = scope.ServiceProvider.GetRequiredService<MongoDBSeeder>();
    await mongodSeeder.SeederInitalization();
    // MongoDB migration ends

    // Neo4j migration starts
    var neo4jDriver = scope.ServiceProvider.GetRequiredService<Neo4j.Driver.IDriver>();
    var neo4jSession = neo4jDriver.AsyncSession();
    var cypherFilePath = "Neo4j/CypherScripts/populate_orders.cypher";

    bool isNeo4jDatabaseEmpty = true;
    var (queryResults, _) = await neo4jDriver
        .ExecutableQuery(@"MATCH (a:Airline {AirlineId: 1})
                            WITH a
                            WHERE a IS NOT NULL
                            RETURN 'Airline with AirlineId 1 already exists' AS message
                            LIMIT 1;")
        .ExecuteAsync();

    if (queryResults.Select(record => record["message"].As<string>()).FirstOrDefault() == "Airline with AirlineId 1 already exists")
    {
        isNeo4jDatabaseEmpty = false;
    }

    if (isNeo4jDatabaseEmpty)
    {
        var cypherQueries = await File.ReadAllTextAsync(cypherFilePath);
        var cypherStatements = cypherQueries.Split(';')
                                            .Select(query => query.Trim())
                                            .Where(query => !string.IsNullOrEmpty(query));

        foreach (var statement in cypherStatements)
        {
            await neo4jSession.RunAsync(statement);
        }
    }

    await neo4jSession.CloseAsync();
    // Neo4j migration ends
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/mssql/swagger.json", "MSSQL API");
    c.SwaggerEndpoint("/swagger/mongodb/swagger.json", "MongoDB API");
    c.SwaggerEndpoint("/swagger/neo4j/swagger.json", "Neo4j API");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();