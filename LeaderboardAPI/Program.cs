using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Repositories;
using LeaderboardBackEnd.Services;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Create Serilog configuration
var log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log_api.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
Log.Logger = log;

// Add SQL Server database
// builder.Services.AddSingleton(_ => new LeaderboardDBContext());
builder.Services.AddSingleton<IDatabaseRepository, DatabaseRepository>(_ => new DatabaseRepository(builder.Configuration.GetConnectionString("Entity")));
// Add MongoDB cache
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));
builder.Services.AddSingleton<IMongoDBRepository, MongoDBRepository>();
// Add services
builder.Services.AddSingleton<ICreationService, CreationService>(_ => new CreationService(_.GetService<IDatabaseRepository>()));
builder.Services.AddSingleton<ILeaderboardService, LeaderboardService>
    (_ => new LeaderboardService(_.GetService<IDatabaseRepository>(), _.GetService<IMongoDBRepository>(), _.GetService<ICreationService>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
