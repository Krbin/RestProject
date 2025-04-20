using RestProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestProject.Services
{
    public interface ISearchService
    {
        Task<List<ApodEntry>> FuzzySearchAsync(string query, List<ApodEntry> entriesToSearch);
    }
}