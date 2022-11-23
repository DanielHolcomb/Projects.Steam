using Core.JwtBuilder;
using Projects.Steam.Repositories;
using Projects.Steam.Repositories.Interfaces;
using Projects.Steam.Services;
using Projects.Steam.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddJwtBuilderAuthentication(builder.Configuration);
builder.Services.AddScoped<ISteamService, SteamService>();
builder.Services.AddHttpClient();

var databaseName = builder.Configuration.GetSection("CosmosDb:DatabaseName").Value;
var containerName = builder.Configuration.GetSection("CosmosDb:ContainerName").Value;
var connectionString = builder.Configuration.GetSection("CosmosDb:ConnectionString").Value;
var client = new Microsoft.Azure.Cosmos.CosmosClient(connectionString);
var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
var cosmosDbService = new ProjectsSteamRepository(client, databaseName, containerName);

builder.Services.AddSingleton<IProjectsSteamRepository>(cosmosDbService);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
