using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaderboardAPI.Controllers;

[Tags("3. Scores")]
[ApiController]
[Route("api/[controller]")]
public class ScoresController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public ScoresController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    [HttpPost("InsertScoreAsync")]
    public async Task<bool> InsertScoreAsync(Score score)
    {
        return await _leaderboardService.InsertScoreAsync(score);
    }

    [HttpGet("GetScoreAsync")]
    public async Task<Score?> GetScoreAsync(int ID)
    {
        return await _leaderboardService.GetScoreAsync(ID);
    }

    [HttpPost("UpdateScoreAsync")]
    public async Task<bool> UpdateScoreAsync(Score score)
    {
        return await _leaderboardService.UpdateScoreAsync(score);
    }

    [HttpDelete("DeleteScoreAsync")]
    public async Task<bool> DeleteScoreAsync(int ID)
    {
        return await _leaderboardService.DeleteScoreAsync(ID);
    }

    [HttpGet("GetAllScoresAsync")]
    public async Task<IEnumerable<Score>?> GetAllScoresAsync()
    {
        return await _leaderboardService.GetAllScoresAsync();
    }

    [HttpGet("SearchAllScoresByOneIDAsync")] // Search by player OR level ID
    public async Task<IEnumerable<Score>?> GetAllScoresAsync(int searchID, bool playerOrLevel)
    {
        return await _leaderboardService.GetAllScoresAsync(searchID, playerOrLevel);
    }

    [HttpGet("SearchAllScoresByTwoIDsAsync")] // Search by player AND level ID
    public async Task<IEnumerable<Score>?> GetAllScoresAsync(int playerID, int levelID)
    {
        return await _leaderboardService.GetAllScoresAsync(playerID, levelID);
    }
}
