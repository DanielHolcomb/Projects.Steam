using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projects.Steam.Models;
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
            apps = apps.OrderBy(o => o.Appid).ToList();
            return Ok(apps.Count);
        }

        [HttpGet]
        [Route("Game")]
        [Authorize]
        public async Task<IActionResult> GetGamesByAppIds([FromQuery(Name = "appId")] List<int> appIds)
        {
            var apps = new List<AppDetails>();

            foreach(int appId in appIds)
            {
                var app = await _steamService.GetSteamAppByAppId(appId);
                apps.Add(app);
            }
            return Ok(apps);
        }

        [HttpPost]
        [Route("Games/Save/{id}")]
        [Authorize]
        public async Task<IActionResult> SaveGameById(int id)
        {
            var app = await _steamService.SaveSteamAppAsync(id);

            if (app == null)
                return BadRequest($"App with id: {id} does not exist");
            return Ok(app);
        }

        [HttpPost]
        [Route("Games/Refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshDb()
        {
            List<string> errorList = new List<string>();
            var retries = 0;
            var maxRetries = 100;
            var newAppsInsertedCount = 0;

            var apps = await _steamService.GetSteamAppsAsync();
            apps = apps.OrderBy(o => o.Appid).ToList();

            foreach (var app in apps)
            {
                try
                {
                    var appDetails = await _steamService.SaveSteamAppAsync(app.Appid);
                    if (appDetails != null)
                        newAppsInsertedCount++;
                    await Task.Delay(5000);
                }
                catch (Exception e)
                {
                    if (retries < maxRetries)
                    {
                        errorList.Add(e.Message);
                        retries++;
                    }
                    else
                        return BadRequest(errorList);
                }
            }

            return Ok(newAppsInsertedCount);
        }
    }
}