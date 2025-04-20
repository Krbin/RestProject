using RestProject.Models;
using RestProject.Data;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace RestProject.Services
{
    public class DatabaseService : IDatabaseService
    {
        private SQLiteAsyncConnection _database;
        private bool _initialized = false;
        private readonly SemaphoreSlim _initializationSemaphore = new SemaphoreSlim(1, 1);

        private async Task EnsureInitializedAsync()
        {
            if (_initialized) return;

            await _initializationSemaphore.WaitAsync();
            try
            {
                if (_initialized) return;

                _database = new SQLiteAsyncConnection(Constants.DatabasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
                await _database.CreateTableAsync<ApodEntry>();
                _initialized = true;
                System.Diagnostics.Debug.WriteLine($"Database initialized at: {Constants.DatabasePath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database initialization failed: {ex.Message}");

                throw;
            }
            finally
            {
                _initializationSemaphore.Release();
            }
        }

        public async Task InitializeAsync()
        {
            await EnsureInitializedAsync();
        }

        public async Task<List<ApodEntry>> GetEntriesAsync()
        {
            await EnsureInitializedAsync();
            return await _database.Table<ApodEntry>().OrderByDescending(x => x.Date).ToListAsync();
        }

        public async Task<ApodEntry> GetEntryByDateAsync(string date)
        {
            await EnsureInitializedAsync();

            return await _database.FindAsync<ApodEntry>(date);

        }

        public async Task<int> SaveEntriesAsync(IEnumerable<ApodEntry> entries)
        {
            await EnsureInitializedAsync();
            if (entries == null || !entries.Any()) return 0;

            return await _database.InsertOrReplaceAsync(entries);

        }

        public async Task<string> GetLastSyncDateAsync()
        {
            await EnsureInitializedAsync();
            var lastEntry = await _database.Table<ApodEntry>().OrderByDescending(x => x.Date).FirstOrDefaultAsync();
            return lastEntry?.Date;
        }

        public async Task<List<ApodEntry>> SearchEntriesAsync(string searchTerm)
        {
            await EnsureInitializedAsync();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {

                return await _database.Table<ApodEntry>().OrderByDescending(x => x.Date).Take(100).ToListAsync();
            }

            var lowerSearchTerm = searchTerm.ToLowerInvariant();

            return await _database.Table<ApodEntry>()
                               .Where(x => x.Title.ToLower().Contains(lowerSearchTerm) || x.Explanation.ToLower().Contains(lowerSearchTerm))
                               .OrderByDescending(x => x.Date)
                               .ToListAsync();
        }

        public async Task<List<IGrouping<int, ApodEntry>>> GetEntriesGroupedByYearAsync()
        {
            await EnsureInitializedAsync();
            var allEntries = await _database.Table<ApodEntry>().OrderBy(x => x.Date).ToListAsync();
            return allEntries.Where(e => e.Year > 0)
                             .GroupBy(e => e.Year)
                             .OrderByDescending(g => g.Key)
                             .ToList();
        }

        public async Task<ApodEntry> GetLastEntryForYearAsync(int year)
        {
            await EnsureInitializedAsync();
            string yearStart = $"{year}-01-01";
            string yearEnd = $"{year}-12-31";
            return await _database.Table<ApodEntry>()
                                  .Where(x => x.Date.CompareTo(yearStart) >= 0 && x.Date.CompareTo(yearEnd) <= 0)
                                  .OrderByDescending(x => x.Date)
                                  .FirstOrDefaultAsync();
        }

        public async Task<List<IGrouping<int, ApodEntry>>> GetEntriesGroupedByMonthAsync(int year)
        {
            await EnsureInitializedAsync();
            string yearStart = $"{year}-01-01";
            string yearEnd = $"{year}-12-31";
            var yearEntries = await _database.Table<ApodEntry>()
                                 .Where(x => x.Date.CompareTo(yearStart) >= 0 && x.Date.CompareTo(yearEnd) <= 0)
                                 .OrderBy(x => x.Date)
                                 .ToListAsync();

            return yearEntries.Where(e => e.Month > 0)
                              .GroupBy(e => e.Month)
                              .OrderBy(g => g.Key)
                              .ToList();
        }

        public async Task<ApodEntry> GetLastEntryForMonthAsync(int year, int month)
        {
            await EnsureInitializedAsync();

            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            string monthStartStr = startDate.ToString("yyyy-MM-dd");
            string monthEndStr = endDate.ToString("yyyy-MM-dd");

            return await _database.Table<ApodEntry>()
                                  .Where(x => x.Date.CompareTo(monthStartStr) >= 0 && x.Date.CompareTo(monthEndStr) <= 0)
                                  .OrderByDescending(x => x.Date)
                                  .FirstOrDefaultAsync();
        }

        public async Task<List<ApodEntry>> GetEntriesForMonthAsync(int year, int month)
        {
            await EnsureInitializedAsync();
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            string monthStartStr = startDate.ToString("yyyy-MM-dd");
            string monthEndStr = endDate.ToString("yyyy-MM-dd");

            return await _database.Table<ApodEntry>()
                                  .Where(x => x.Date.CompareTo(monthStartStr) >= 0 && x.Date.CompareTo(monthEndStr) <= 0)
                                  .OrderByDescending(x => x.Date)
                                  .ToListAsync();
        }
    }
}