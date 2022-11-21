using System.Text.Json.Serialization;

namespace Projects.Steam.Models
{
    public class AllApps
    {
        [JsonPropertyName("applist")]
        public AppList Applist { get; set; }
    }

    public class App
    {
        [JsonPropertyName("appid")]
        public int Appid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class AppList
    {
        [JsonPropertyName("apps")]
        public List<App> Apps { get; set; }
    }
}
