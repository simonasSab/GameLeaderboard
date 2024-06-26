using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaderboardAPI.Controllers;

[Tags("2. Players")]
[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public PlayersController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

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
}
