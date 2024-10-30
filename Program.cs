using Database_project.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Add Database context
builder.Services.AddDbContextFactory<DatabaseContext>(options =>
{
    options.UseSqlServer("Data Source=127.0.0.1;Database=DineTime;User ID=sa;Password=Password123;Trusted_Connection=False;Encrypt=True;TrustServerCertificate=True;");
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();