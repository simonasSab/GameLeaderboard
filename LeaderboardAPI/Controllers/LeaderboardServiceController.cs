using Microsoft.AspNetCore.Mvc;
using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole;
using Serilog.Sinks.File;

namespace LeaderboardAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardServiceController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public LeaderboardServiceController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    // Cache cleaning

    [HttpPost("ToggleCacheCleaning")]
    public void ToggleCacheCleaning(int cachePeriod)
    {
        _leaderboardService.ToggleCacheCleaning(cachePeriod);
    }

    // ------------ Random object insertion ------------

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

    // ------------ Level ------------

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

    // ------------ Player ------------

    [HttpPost("InsertPlayerAsync")]
    public async Task<bool> InsertPlayerAsync(Player player)
    {
        return await _leaderboardService.InsertPlayerAsync(player);
    }

    [HttpGet("GetPlayerAsync")]
    public async Task<Player?> GetPlayerAsync(int ID)
    {
        return await _leaderboardService.GetPlayerAsync(ID);
    }

    [HttpPost("UpdatePlayerAsync")]
    public async Task<bool> UpdatePlayerAsync(Player player)
    {
        return await _leaderboardService.UpdatePlayerAsync(player);
    }

    [HttpDelete("DeletePlayerAsync")]
    public async Task<bool> DeletePlayerAsync(int ID)
    {
        return await _leaderboardService.DeletePlayerAsync(ID);
    }

    [HttpGet("GetAllPlayersAsync")]
    public async Task<IEnumerable<Player>?> GetAllPlayersAsync()
    {
        return await _leaderboardService.GetAllPlayersAsync();
    }

    [HttpGet("SearchAllPlayersAsync")] // Search
    public async Task<IEnumerable<Player>?> GetAllPlayersAsync(string phrase) 
    {
        return await _leaderboardService.GetAllPlayersAsync(phrase);
    }

    // ------------ Score ------------

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