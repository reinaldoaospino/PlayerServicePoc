using Dapr.Actors.Runtime;

namespace PlayerService.Actors;

public class PlayerActor(ActorHost host) : Actor(host), IPlayerActor
{
    private const string StateKey = "points";
    
    public async Task AddPointsAsync(int points)
    {
        var current = await StateManager.GetStateAsync<int>(StateKey);
        current += points;
        await StateManager.SetStateAsync(StateKey, current);
    }

    public Task<int> GetPointsAsync()
    {
       return StateManager.GetStateAsync<int>(StateKey);
    }

    protected override async Task OnActivateAsync()
    {
        if (!await StateManager.ContainsStateAsync(StateKey))
        {
            await StateManager.SetStateAsync(StateKey, 0);
        }
    }
}