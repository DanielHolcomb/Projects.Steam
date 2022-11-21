using System.Text.Json.Serialization;

namespace Projects.Steam.Models
{
    public class AppDetailsRoot
    {
        public AppDetails AppDetails { get; set; }
    }

    public class AppDetails
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
