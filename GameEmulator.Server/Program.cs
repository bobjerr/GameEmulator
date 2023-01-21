using Orleans.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder
        .UseLocalhostClustering()
        .UseMongoDBClient("mongodb://localhost:27017")
        .AddMongoDBGrainStorage("GameStateStore", options =>
        {
            options.DatabaseName = "GameDB";
        })
        .Configure<GrainCollectionOptions>(options =>
        {
            options.CollectionAge = TimeSpan.FromMinutes(5);
        });
});

var app = builder.Build();

app.Run();
