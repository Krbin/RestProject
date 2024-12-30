using BlazorRestProject.Database;
using BlazorRestProject.Models;
using Newtonsoft.Json;

namespace BlazorRestProject.Services
{
    public class ApodService
    {
        private readonly HttpClient _httpClient;

        private const string BaseApiUrl = "https://api.nasa.gov/planetary/apod";
        private readonly string ApiKey = new Func<string>(() =>
        {
            var configJson = File.ReadAllText("config.json");
            var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(configJson);

            if (!config.TryGetValue("NASA_API_KEY", out var apiKey) || string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("API key not found in configuration.");
            }
            return apiKey;
        })();

        private readonly SqliteService _sqliteService;
        public ApodService(SqliteService sqliteService)
        {
            _sqliteService = sqliteService;
            _httpClient = new HttpClient();
        }

        public async Task<List<ApodModel>> GetApodImages(string startDate, string endDate)
        {
            List<ApodModel> apodImages = new List<ApodModel>();
            var apodResponses = new List<ApodResponse>();

            using (HttpClient client = _httpClient)
            {
                string url = $"{BaseApiUrl}?api_key={ApiKey}&start_date={startDate}&end_date={endDate}";
                HttpResponseMessage response = await client.GetAsync(url);


                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JsonConvert.DeserializeObject<List<ApodResponse>>(content);
                    foreach (var item in apodResponses)
                    {
                        apodImages.Add(CreateModelFromApodResponse(item));
                    }
                }
                else
                {
                    Console.WriteLine("Error fetching data");
                }
            }

            return apodImages;
        }

        public async Task<List<ApodModel>> GetApodImagesForDateRange(int year, int? month = null, int? day = null)
        {
            string startDate, endDate;

            if (day.HasValue)
            {
                startDate = endDate = $"{year:D4}-{month.Value:D2}-{day.Value:D2}";
            }
            else if (month.HasValue)
            {
                startDate = $"{year:D4}-{month.Value:D2}-01";
                int daysInMonth = DateTime.DaysInMonth(year, month.Value);
                endDate = $"{year:D4}-{month.Value:D2}-{daysInMonth:D2}";
            }
            else
            {
                startDate = $"{year:D4}-01-01";
                endDate = $"{year:D4}-12-31";
            }

            return await GetApodImages(startDate, endDate);
        }
        public async Task FetchAndStoreApodImagesOfAllTimes()
        {
            DateTime startDate = new DateTime(1995, 6, 16);
            DateTime endDate = DateTime.UtcNow.Date;
            const int maxDaysPerRequest = 100;

            while (startDate <= endDate)
            {
                DateTime batchEndDate = startDate.AddDays(maxDaysPerRequest - 1);
                if (batchEndDate > endDate)
                {
                    batchEndDate = endDate;
                }
                await FetchAndStoreApodImagesForRange(startDate.ToString("yyyy-MM-dd"), batchEndDate.ToString("yyyy-MM-dd"));

                startDate = batchEndDate.AddDays(1);
            }
        }
        public async Task FetchAndStoreApodImagesForRange(string startDate, string endDate)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{BaseApiUrl}?api_key={ApiKey}&start_date={startDate}&end_date={endDate}";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var apodResponses = JsonConvert.DeserializeObject<List<ApodResponse>>(content);
                    var apodModels = new List<ApodModel>();
                    foreach (var item in apodResponses)
                    {
                        apodModels.Add(CreateModelFromApodResponse(item));
                    }

                    await _sqliteService.InsertAllDataAsync(apodModels);
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Failed to fetch APOD data: {error}");
                }
            }
        }
        public async Task<List<ApodModel>> GetAllApodModelsAsync()
        {
            return await _sqliteService.GetAllDataAsync<ApodModel>(); ;
        }

        public static ApodModel CreateModelFromApodResponse(ApodResponse resp)
        {
            ApodModel output = new ApodModel();
            //output.Copyright = resp.Copyright;
            //output.Date = resp.Date;
            //output.ExplanationEnglish = resp.Explanation;
            //output.HdUrl = resp.HdUrl;
            //output.MediaType = resp.MediaType;
            //output.ServiceVersion = resp.ServiceVersion;
            //output.TitleEnglish = resp.Title;
            //output.Url = resp.Url;

            if (DateTime.TryParse(resp.Date, out DateTime parsedDate))
            {
                Func<DateTime, string> getYearAsWords = date => date.ToString("yyyy");
                Func<DateTime, string> getMonthAsWords = date => date.ToString("MMMM");

                //output.Year = getYearAsWords(parsedDate);
                //output.Month = getMonthAsWords(parsedDate);
            }
            else
            {
                //output.Year = "Invalid Date";
                //output.Month = "Invalid Date";
            }

            return output;
        }
    }
}
