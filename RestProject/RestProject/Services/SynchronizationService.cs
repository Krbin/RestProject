using Microsoft.Maui.Storage;
using RestProject.Data;
using RestProject.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestProject.Services
{
    public class SynchronizationService : ISynchronizationService
    {
        private readonly INasaApiService _nasaApiService;
        private readonly IDatabaseService _databaseService;
        private static readonly SemaphoreSlim _syncSemaphore = new SemaphoreSlim(1, 1);
        private volatile bool _isSyncing = false;
        private string _syncStatus = "Idle";

        public bool IsSyncing => _isSyncing;
        public string SyncStatus
        {
            get => _syncStatus;
            private set
            {
                if (_syncStatus != value)
                {
                    _syncStatus = value;
                    OnSyncProgressChanged();
                }
            }
        }
        public event EventHandler SyncProgressChanged;

        public SynchronizationService(INasaApiService nasaApiService, IDatabaseService databaseService)
        {
            _nasaApiService = nasaApiService;
            _databaseService = databaseService;
        }

        public async Task StartSyncIfNeededAsync()
        {

            if (!await _syncSemaphore.WaitAsync(0))
            {
                System.Diagnostics.Debug.WriteLine("Sync already in progress.");
                return;
            }

            _isSyncing = true;
            try
            {
                SyncStatus = "Initializing sync...";
                await _databaseService.InitializeAsync();

                string lastDbDateStr = await _databaseService.GetLastSyncDateAsync();
                string lastPrefDateStr = Preferences.Get(Constants.SyncStateKey, null);

                DateTime startDate;
                DateTime lastDbDate = DateTime.MinValue;
                DateTime lastPrefDate = DateTime.MinValue;

                if (!string.IsNullOrEmpty(lastDbDateStr)) DateTime.TryParse(lastDbDateStr, out lastDbDate);
                if (!string.IsNullOrEmpty(lastPrefDateStr)) DateTime.TryParse(lastPrefDateStr, out lastPrefDate);

                DateTime lastKnownDate = lastDbDate > lastPrefDate ? lastDbDate : lastPrefDate;

                if (lastKnownDate > DateTime.MinValue)
                {
                    startDate = lastKnownDate.AddDays(1);
                }
                else
                {
                    startDate = Constants.ApiStartDate;
                }

                DateTime today = DateTime.Today;
                if (startDate > today)
                {
                    SyncStatus = "Database is up-to-date.";
                    System.Diagnostics.Debug.WriteLine("Sync Check: Database up to date.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Starting sync from: {startDate:yyyy-MM-dd}");

                while (startDate <= today)
                {

                    DateTime endDate = startDate.AddDays(Constants.ApiRequestsPerBatch - 1);
                    if (endDate > today)
                    {
                        endDate = today;
                    }

                    SyncStatus = $"Fetching: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}";

                    var entries = await _nasaApiService.GetApodDataAsync(startDate, endDate);

                    if (entries != null && entries.Count > 0)
                    {
                        SyncStatus = $"Saving {entries.Count} entries...";

                        var imageEntries = entries.Where(e => e.IsImage).ToList();
                        if (imageEntries.Any())
                        {
                            await _databaseService.SaveEntriesAsync(imageEntries);

                            string lastSavedDate = imageEntries.Max(e => e.Date);
                            Preferences.Set(Constants.SyncStateKey, lastSavedDate);
                            System.Diagnostics.Debug.WriteLine($"Saved entries up to {lastSavedDate}");
                        }
                        else
                        {

                            Preferences.Set(Constants.SyncStateKey, endDate.ToString("yyyy-MM-dd"));
                            System.Diagnostics.Debug.WriteLine($"No images found in range, advancing marker to {endDate:yyyy-MM-dd}");
                        }
                    }
                    else
                    {

                        Preferences.Set(Constants.SyncStateKey, endDate.ToString("yyyy-MM-dd"));
                        System.Diagnostics.Debug.WriteLine($"No data received for range, advancing marker to {endDate:yyyy-MM-dd}");
                    }

                    startDate = endDate.AddDays(1);

                    if (startDate <= today)
                    {
                        SyncStatus = $"Waiting {Constants.DelayBetweenBatchesMs / 1000}s...";
                        await Task.Delay(Constants.DelayBetweenBatchesMs);
                    }
                }

                SyncStatus = "Sync complete.";
                System.Diagnostics.Debug.WriteLine("Synchronization finished.");
            }
            catch (Exception ex)
            {
                SyncStatus = $"Sync Error: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Synchronization failed: {ex}");

            }
            finally
            {
                _isSyncing = false;
                _syncSemaphore.Release();
                OnSyncProgressChanged();
            }
        }

        protected virtual void OnSyncProgressChanged()
        {

            SyncProgressChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}