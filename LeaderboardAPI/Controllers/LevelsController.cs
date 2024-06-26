using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaderboardAPI.Controllers;

[Tags("1. Levels")]
[ApiController]
[Route("api/[controller]")]
public class LevelsController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public LevelsController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    [HttpPost("InsertLevelAsync")]
    public async Task<bool> InsertLevelAsync(Level level)
    {
        return await _leaderboardService.InsertLevelAsync(level);
    }

    [HttpGet("GetLevelAsync")]
    public async Task<Level?> GetLevelAsync(int ID)
    {
        return await _leaderboardService.GetLevelAsync(ID);
    }

    [HttpPost("UpdateLevelAsync")]
    public async Task<bool> UpdateLevelAsync(Level level)
    {
        return await _leaderboardService.UpdateLevelAsync(level);
    }

    [HttpDelete("DeleteLevelAsync")]
    public async Task<bool> DeleteLevelAsync(int ID)
    {
        return await _leaderboardService.DeleteLevelAsync(ID);
    }

    [HttpGet("GetAllLevelsAsync")]
    public async Task<IEnumerable<Level>?> GetAllLevelsAsync()
    {
        return await _leaderboardService.GetAllLevelsAsync();
    }    
}