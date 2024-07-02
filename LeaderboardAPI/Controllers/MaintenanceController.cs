using LeaderboardBackEnd.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LeaderboardAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public MaintenanceController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    // Cache cleaning
    [HttpPost("ToggleCacheCleaning")]
    public void ToggleCacheCleaning(int cachePeriod)
    {
        _leaderboardService.ToggleCacheCleaning(cachePeriod);
    }

    // MongoDB backup
    [HttpPost("MongoDBBackupSave")]
    public async Task SaveBackup()
    {
        await _leaderboardService.SaveBackup();
    }
    [HttpPost("MongoDBBackupRestore")]
    public async Task LoadBackup()
    {
        await _leaderboardService.LoadBackup();
    }

    // Random object insertion
    [HttpGet("InsertRandomLevelAsync")]
    public async Task<bool> InsertRandomLevelAsync()
    {
        return await _leaderboardService.InsertRandomLevelAsync();
    }
    [HttpGet("InsertRandomPlayerAsync")]
    public async Task<bool> InsertRandomPlayerAsync()
    {
        return await _leaderboardService.InsertRandomPlayerAsync();
    }
    [HttpGet("InsertRandomScoreAsync")]
    public async Task<bool> InsertRandomScoreAsync()
    {
        return await _leaderboardService.InsertRandomScoreAsync();
    }
}