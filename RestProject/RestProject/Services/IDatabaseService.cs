using RestProject.Models;
using System.Collections.Generic;
using System.Linq; // For IGrouping
using System.Threading.Tasks;

namespace RestProject.Services
{
    public interface IDatabaseService
    {
        Task InitializeAsync();
        Task<List<ApodEntry>> GetEntriesAsync();
        Task<ApodEntry> GetEntryByDateAsync(string date);
        Task<int> SaveEntriesAsync(IEnumerable<ApodEntry> entries);
        Task<string> GetLastSyncDateAsync(); // Get the date of the most recent entry
        Task<List<ApodEntry>> SearchEntriesAsync(string searchTerm);
        Task<List<IGrouping<int, ApodEntry>>> GetEntriesGroupedByYearAsync();
        Task<List<IGrouping<int, ApodEntry>>> GetEntriesGroupedByMonthAsync(int year);
        Task<List<ApodEntry>> GetEntriesForMonthAsync(int year, int month);
        Task<ApodEntry> GetLastEntryForYearAsync(int year);
        Task<ApodEntry> GetLastEntryForMonthAsync(int year, int month);
    }
}