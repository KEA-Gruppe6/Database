using Database_project.Core;
using Database_project.Core.Interfaces;
using Database_project.Core.MongoDB.Services;
using Database_project.Core.Services;
using Database_project.MongoDB_Query;
using Database_project.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using AirlineService = Database_project.Services.AirlineService;

var builder = WebApplication.CreateBuilder(args);

// Configure MongoDbSettings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Add Database context
var connectionString = builder.Configuration.GetConnectionString("MSSQL") ?? Environment.GetEnvironmentVariable("MSSQL");
var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDB") ?? Environment.GetEnvironmentVariable("MongoDB");

builder.Services.AddDbContextFactory<DatabaseContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Setup for MongoDb driver
builder.Services.Configure<MongoDbSettings>(options =>
{
    options.ConnectionString = mongoDbConnectionString;
    options.DatabaseName = "AirportDB";
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

// Register services
builder.Services.AddScoped<IAirlineService, AirlineService>();
builder.Services.AddScoped<Database_project.Core.Services.PlaneService>();
builder.Services.AddScoped<Database_project.Core.Services.OrderService>();

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

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
