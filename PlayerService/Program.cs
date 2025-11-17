using Dapr.Actors;
using Dapr.Actors.Client;
using PlayerService.Actors;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5000");

builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<PlayerActor>();
    options.HttpEndpoint = "http://playerservice-dapr:3500";
    options.DaprApiToken = string.Empty; 
});

var app = builder.Build();

app.MapOpenApi();
app.MapActorsHandlers();

app.MapGet("/", () => "PlayerService running");

app.MapPost("/player/{id}/add", async (string id, int points, IActorProxyFactory proxyFactory, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Adding {Points} points to player {PlayerId}", points, id);
        
        var proxy = proxyFactory.CreateActorProxy<IPlayerActor>(
            new ActorId(id),
            "PlayerActor"
        );

        await proxy.AddPointsAsync(points);
        
        logger.LogInformation("Successfully added points to player {PlayerId}", id);
        return Results.Ok(new { message = $"Added {points} points to player {id}" });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error adding points to player {PlayerId}", id);
        return Results.Problem($"Error: {ex.Message}");
    }
});

app.MapGet("/player/{id}/points", async (string id, IActorProxyFactory proxyFactory, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Getting points for player {PlayerId}", id);
        
        var proxy = proxyFactory.CreateActorProxy<IPlayerActor>(
            new ActorId(id),
            "PlayerActor"
        );

        var total = await proxy.GetPointsAsync();
        
        logger.LogInformation("Player {PlayerId} has {Points} points", id, total);
        return Results.Ok(new { playerId = id, totalPoints = total });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error getting points for player {PlayerId}", id);
        return Results.Problem($"Error: {ex.Message}");
    }
});

app.Run();