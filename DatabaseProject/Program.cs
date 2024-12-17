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

// Configure MongoDbSettings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Add Database context
var connectionString = builder.Configuration.GetConnectionString("MSSQL") ?? Environment.GetEnvironmentVariable("MSSQL");
var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDB") ?? Environment.GetEnvironmentVariable("MongoDB");
var mongoDbDatabaseName = builder.Configuration.GetConnectionString("DatabaseName");
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
builder.Services.AddSingleton(GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "Password123")));

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
builder.Services.AddScoped<Database_project.Neo4j.Services.AirlineService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.OrderService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.TicketService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.CustomerService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.LuggageService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.FlightrouteService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.AirportService>();
builder.Services.AddScoped<Database_project.Neo4j.Services.PlaneService>();

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
    var addUsersService = scope.ServiceProvider.GetRequiredService<AddUsers>();
    addUsersService.CreateMongoUsers();  // Run the user creation script
    var mongodSeeder = scope.ServiceProvider.GetRequiredService<MongoDBSeeder>();
    await mongodSeeder.SeederInitalization();
    // MongoDB migration ends
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