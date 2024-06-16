using Microsoft.AspNetCore.Mvc;
using LeaderboardBackEnd.Contracts;
using LeaderboardBackEnd.Services;
using Microsoft.Extensions.Hosting;

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
}