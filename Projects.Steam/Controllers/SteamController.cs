using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Projects.Steam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SteamController : ControllerBase
    {
        private readonly ILogger<SteamController> _logger;

        public SteamController(ILogger<SteamController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("Developer/Games")]
        [Authorize]
        public async Task<IActionResult> GetGamesByDeveloper()
        {
            return Ok();
        }
    }
}