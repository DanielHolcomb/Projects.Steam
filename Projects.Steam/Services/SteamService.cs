using Projects.Steam.Models;
using Projects.Steam.Repositories.Interfaces;
using Projects.Steam.Services.Interfaces;
using Projects.Steam.Utils;
using System.Text.Json;

namespace Projects.Steam.Services
{
    public class SteamService : ISteamService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _steamWebApiKey;
        private readonly IProjectsSteamRepository _projectsSteamRepository;

        public SteamService(IHttpClientFactory httpClientFactory, IConfiguration config, IProjectsSteamRepository projectsSteamRepository)
        {
            _httpClientFactory = httpClientFactory;
            _steamWebApiKey = config.GetSection("Steam:ApiKey").Value;
            _projectsSteamRepository = projectsSteamRepository;
        }

        public async Task<List<App>> GetSteamAppsAsync()
        {
            var url = $"https://api.steampowered.com/ISteamApps/GetAppList/v2";
            var allApps = await SteamUtils.SendSteamRequestAsync<AllApps>(_httpClientFactory, url);
            var listOfApps = new List<App>();
            if (allApps != null)
            {
                foreach (var app in allApps.Applist.Apps)
                {
                    if (app.Name != null && app.Name != "")
                    {
                        listOfApps.Add(app);
                    }
                }
            }
            return listOfApps;
        }

        public async Task<AppDetails?> SaveSteamAppAsync(int appId)
        {
            var url = $"https://store.steampowered.com/api/appdetails?appids={appId}";
            var appRoot = await SteamUtils.SendSteamRequestAsyncForDyna<AppDetails>(_httpClientFactory, url);
            var app = appRoot[appId.ToString()];
            app.id = appId.ToString();
            await _projectsSteamRepository.InsertSteamAppAsync(app);
            return app;
        }
    }
}
