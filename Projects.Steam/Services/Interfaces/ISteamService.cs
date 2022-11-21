
using Projects.Steam.Models;

namespace Projects.Steam.Services.Interfaces
{
    public interface ISteamService
    {
        public Task<AllApps> GetSteamAppsAsync();
    }
}
