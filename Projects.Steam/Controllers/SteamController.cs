using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projects.Steam.Services.Interfaces;

namespace Projects.Steam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SteamController : ControllerBase
    {
        private readonly ILogger<SteamController> _logger;
        private readonly ISteamService _steamService;

        public SteamController(ILogger<SteamController> logger, ISteamService steamService)
        {
            _logger = logger;
            _steamService = steamService;
        }

        [HttpGet]
        [Route("Developer/Games")]
        [Authorize]
        public async Task<IActionResult> GetGamesByDeveloper()
        {
            var appOwnership = await _steamService.GetSteamAppsAsync();
            return Ok(appOwnership);
        }
    }
}