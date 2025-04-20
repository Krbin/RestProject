using RestProject.Models;
using RestProject.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace RestProject.Services
{
    public class NasaApiService : INasaApiService
    {

        private static readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://api.nasa.gov/") };
        private readonly JsonSerializerOptions _serializerOptions;

        public NasaApiService()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };
        }

        public async Task<List<ApodEntry>> GetApodDataAsync(DateTime startDate, DateTime endDate)
        {
            string startDateStr = startDate.ToString("yyyy-MM-dd");
            string endDateStr = endDate.ToString("yyyy-MM-dd");
            string apiKey = Constants.NasaApiKey;

            string url = $"planetary/apod?api_key={apiKey}&start_date={startDateStr}&end_date={endDateStr}&thumbs=true";

            System.Diagnostics.Debug.WriteLine($"NASA API Request: {url}");

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    System.Diagnostics.Debug.WriteLine("NASA API Rate Limit Exceeded. Waiting and retrying might be needed.");

                    return new List<ApodEntry>();
                }

                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"NASA API Response (sample): {jsonResponse.Substring(0, Math.Min(jsonResponse.Length, 200))}");

                if (jsonResponse.TrimStart().StartsWith("["))
                {
                    var entries = JsonSerializer.Deserialize<List<ApodEntry>>(jsonResponse, _serializerOptions);
                    return entries?.Where(e => e != null && !string.IsNullOrEmpty(e.Date)).ToList() ?? new List<ApodEntry>();
                }
                else if (jsonResponse.TrimStart().StartsWith("{") && startDate == endDate)
                {
                    var entry = JsonSerializer.Deserialize<ApodEntry>(jsonResponse, _serializerOptions);
                    if (entry != null && !string.IsNullOrEmpty(entry.Date))
                    {

                        if (string.IsNullOrWhiteSpace(entry.Date)) entry.Date = startDateStr;
                        return new List<ApodEntry> { entry };
                    }
                    return new List<ApodEntry>();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Unexpected JSON format from NASA API: {jsonResponse}");
                    return new List<ApodEntry>();
                }
            }
            catch (HttpRequestException httpEx)
            {
                System.Diagnostics.Debug.WriteLine($"API Request Error: {httpEx.StatusCode} - {httpEx.Message}");
                return new List<ApodEntry>();
            }
            catch (JsonException jsonEx)
            {
                System.Diagnostics.Debug.WriteLine($"JSON Deserialization Error: {jsonEx.Message}");
                return new List<ApodEntry>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An unexpected error occurred fetching APOD data: {ex.Message}");
                return new List<ApodEntry>();
            }
        }
    }
}