
using Projects.Steam.Models;

namespace Projects.Steam.Services.Interfaces
{
    public interface ISteamService
    {
        public Task<List<App>> GetSteamAppsAsync();
    }
}
