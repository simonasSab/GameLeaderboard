using LeaderboardBackEnd.Models;

namespace LeaderboardBackEnd.Contracts;

public interface ICreationService
{
    Task<Player> CreateRandomPlayerAsync();
    Task<Score> CreateRandomScoreAsync();
}
