using Dapr.Actors;

namespace PlayerService.Actors;

public interface IPlayerActor : IActor
{ 
    Task AddPointsAsync(int points);
    Task<int> GetPointsAsync();
}