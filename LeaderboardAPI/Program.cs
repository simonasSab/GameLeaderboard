using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Repositories;
using LeaderboardBackEnd.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SQL Server database
builder.Services.AddSingleton<IDatabaseRepository, DatabaseRepository>(_ => new DatabaseRepository());
// Add MongoDB cache
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));
builder.Services.AddSingleton<IMongoDBRepository, MongoDBRepository>();
// Add RentService
builder.Services.AddSingleton<ILeaderboardService, LeaderboardService>(_ => new LeaderboardService(_.GetService<IDatabaseRepository>(), _.GetService<IMongoDBRepository>()));


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
