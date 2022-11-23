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
            apps = apps.OrderBy(o => o.Appid).ToList();
            return Ok(apps.Count);
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
                    await Task.Delay(1000);
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