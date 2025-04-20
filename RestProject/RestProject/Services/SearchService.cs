using FuzzySharp;
using RestProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestProject.Services
{
    public class SearchService : ISearchService
    {

        private const int FuzzySearchThreshold = 65;

        public Task<List<ApodEntry>> FuzzySearchAsync(string query, List<ApodEntry> entriesToSearch)
        {

            return Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(query))
                {

                    return new List<ApodEntry>();
                }

                if (entriesToSearch == null || !entriesToSearch.Any())
                {
                    return new List<ApodEntry>();
                }

                var choices = entriesToSearch.Select(e => $"{e.Title ?? ""} {e.Explanation ?? ""}").ToList();

                var results = Process.ExtractTop(
                    query: query,
                    choices: choices,
                    limit: entriesToSearch.Count,
                    cutoff: FuzzySearchThreshold
                );

                var matchedEntries = results.Select(result => entriesToSearch[result.Index]).ToList();

                return matchedEntries;
            });
        }
    }
}