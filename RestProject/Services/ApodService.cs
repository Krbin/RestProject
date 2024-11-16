using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using RestProject.Models;

namespace RestProject.Services
{
    class ApodService
    {
        private readonly HttpClient _httpClient;
        private string ApiKey; // Replace with your NASA API key
        private const string BaseUrl = "https://api.nasa.gov/planetary/apod";

        public ApodService()
        {
            var configJson = File.ReadAllText("config.json");
            var config = JsonSerializer.Deserialize<Dictionary<string, string>>(configJson);

            if (!config.TryGetValue("NASA_API_KEY", out var apiKey) || string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("API key not found in configuration.");
            }
            ApiKey = apiKey;

            _httpClient = new HttpClient();
        }

        public async Task<ApodModel> GetApodAsync()
        {
            var url = $"{BaseUrl}?api_key={ApiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApodModel>(json);
        }
    }
}
