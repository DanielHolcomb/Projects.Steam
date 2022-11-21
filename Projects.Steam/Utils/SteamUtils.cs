using System.Text.Json;

namespace Projects.Steam.Utils
{
    public static class SteamUtils
    {
        public async static Task<T?> SendSteamRequestAsync<T>(IHttpClientFactory _httpClientFactory, string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            var result = default(T);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<T>(contentStream);
            }

            return result;
        }

        public async static Task<Dictionary<string, T>> SendSteamRequestAsyncForDyna<T>(IHttpClientFactory _httpClientFactory, string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            var result = new Dictionary<string, T>();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                result = JsonSerializer.Deserialize<Dictionary<string, T>>(contentStream);
            }

            return result;
        }
    }
}
