using Database_project.Core;
using Database_project.Core.Interfaces;
using Database_project.MongoDB_Query;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

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

builder.Services.AddControllers();
// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddScoped<Database_project.Core.MongoDB.Services.DepartureService>();
builder.Services.AddScoped<Database_project.Core.MongoDB.Services.LuggageService>();
builder.Services.AddScoped<Database_project.Core.MongoDB.Services.PlaneService>();
builder.Services.AddScoped<Database_project.Core.MongoDB.Services.MaintenanceService>();

// Register AddUsers as scoped service
builder.Services.AddScoped<AddUsers>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
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
    var sqlFilePath = "passportlengthtrigger.sql";
    var sqlQuery = File.ReadAllText(sqlFilePath);
    dbContext.Database.ExecuteSqlRaw("IF OBJECT_ID('trg_ValidatePassportNumber', 'TR') IS NOT NULL DROP TRIGGER trg_ValidatePassportNumber;");
    dbContext.Database.ExecuteSqlRaw(sqlQuery);

    // Run seed SQL query
    sqlFilePath = "populatedb.sql";
    sqlQuery = File.ReadAllText(sqlFilePath);
    dbContext.Database.ExecuteSqlRaw(sqlQuery);
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// Create a scope to resolve AddUsers (because it's a scoped service)
using (var scope = app.Services.CreateScope())
{
    var addUsersService = scope.ServiceProvider.GetRequiredService<AddUsers>();
    addUsersService.CreateMongoUsers();  // Run the user creation script
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();