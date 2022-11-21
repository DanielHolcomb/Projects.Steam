using Projects.Steam.Models;
using Projects.Steam.Services.Interfaces;
using Projects.Steam.Utils;
using System.Text.Json;

namespace Projects.Steam.Services
{
    public class SteamService : ISteamService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _steamWebApiKey;

        public SteamService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _steamWebApiKey = config.GetSection("Steam:ApiKey").Value;
        }

        public async Task<List<App>> GetSteamAppsAsync()
        {
            var url = $"https://api.steampowered.com/ISteamApps/GetAppList/v2";
            var allApps = await SteamUtils.SendSteamRequestAsync<AllApps>(_httpClientFactory, url);
            var listOfApps = new List<App>();
            foreach (var app in allApps.Applist.Apps)
            {
                if (app.Name != null && app.Name != "")
                {
                    listOfApps.Add(app);
                }
                //var detailsUrl = $"https://store.steampowered.com/api/appdetails?appids={app.Appid}";
                //var thisApp = await SteamUtils.SendSteamRequestAsyncForDyna<AppDetails>(_httpClientFactory, detailsUrl);
                //foreach (var t in thisApp.Values)
                //{
                //    if (t?.Data?.Name != null && t.Data.Name == "Kestrel")
                //        return new AllApps()
                //        {
                //            Applist = new AppList()
                //            {
                //                Apps = new List<App>()
                //                {
                //                    app
                //                }
                //            }
                //        };
                //}
            }
            return listOfApps;
        }
    }
}
