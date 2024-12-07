using Database_project.Core;
using Database_project.Core.Interfaces;
using Database_project.Core.Services;
using Database_project.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

//Add Database context
var connectionString = builder.Configuration.GetConnectionString("MSSQL") ?? Environment.GetEnvironmentVariable("MSSQL");
builder.Services.AddDbContextFactory<DatabaseContext>(options =>
{
    options.UseSqlServer(connectionString);
});

//TODO: Authentication of API calls

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
            return routeTemplate != null && routeTemplate.StartsWith("api/mongodb");
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

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAirlineService, AirlineService>();
builder.Services.AddScoped<IAirportService, AirportService>();
builder.Services.AddScoped<ITicketTypeService, TicketTypeService>();
builder.Services.AddScoped<IPlaneService, PlaneService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IFlightrouteService, FlightrouteService>();
builder.Services.AddScoped<ILuggageService, LuggageService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddScoped<ITicketService, TicketService>();

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