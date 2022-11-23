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
        [Route("Games")]
        [Authorize]
        public async Task<IActionResult> GetAllGames()
        {
            var apps = await _steamService.GetSteamAppsAsync();
            return Ok(apps);
        }

        [HttpGet]
        [Route("Games/Count")]
        [Authorize]
        public async Task<IActionResult> GetTotalGames()
        {
            var apps = await _steamService.GetSteamAppsAsync();
            return Ok(apps.Count);
        }

        [HttpPost]
        [Route("Games/Save/{id}")]
        public async Task<IActionResult> SaveGameById(int id)
        {
            var app = await _steamService.SaveSteamAppAsync(id);

            if (app == null)
                return BadRequest($"App with id: {id} does not exist");
            return Ok(app);
        }
    }
}