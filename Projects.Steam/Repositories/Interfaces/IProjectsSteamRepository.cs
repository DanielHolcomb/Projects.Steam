using Projects.Steam.Models;

namespace Projects.Steam.Repositories.Interfaces
{
    public interface IProjectsSteamRepository
    {
        public Task InsertSteamAppAsync(AppDetails appDetails);
    }
}
