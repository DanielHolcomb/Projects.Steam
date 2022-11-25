
using Projects.Steam.Models;

namespace Projects.Steam.Services.Interfaces
{
    public interface ISteamService
    {
        public Task<List<App>> GetSteamAppsAsync();

        public Task<AppDetails> SaveSteamAppAsync(int id);

        public Task<AppDetails> GetSteamAppByAppId(int appId);
    }
}
