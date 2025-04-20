using Microsoft.AspNetCore.Components;
using RestProject.Models;
using RestProject.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace RestProject.ViewModels
{
    public class SearchViewModel : BaseViewModel, IDisposable
    {
        private readonly IDatabaseService _databaseService;
        private readonly ISearchService _searchService;
        private readonly NavigationManager _navigationManager;
        private readonly ISynchronizationService _syncService;

        private string _searchTerm;
        private string _syncStatus;
        private bool _isSyncing;
        private ObservableCollection<ApodEntry> _searchResults;
        private List<ApodEntry> _allEntriesCache;
        private bool _isLoading = true;
        private bool _isSearching = false;
        private Timer _debounceTimer;
        private CancellationTokenSource _searchCts;

        public string SearchTerm
        {
            get => _searchTerm;

            set => SetProperty(ref _searchTerm, value, onChanged: () => ScheduleSearch());
        }

        public ObservableCollection<ApodEntry> SearchResults
        {
            get => _searchResults;
            private set => SetProperty(ref _searchResults, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
        public bool IsSearching
        {
            get => _isSearching;
            set => SetProperty(ref _isSearching, value);
        }

        public string SyncStatus
        {
            get => _syncStatus;
            set => SetProperty(ref _syncStatus, value);
        }
        public bool IsSyncing
        {
            get => _isSyncing;
            set => SetProperty(ref _isSyncing, value);
        }

        public SearchViewModel(IDatabaseService databaseService, ISearchService searchService, NavigationManager navigationManager, ISynchronizationService syncService)
        {
            _databaseService = databaseService;
            _searchService = searchService;
            _navigationManager = navigationManager;
            _syncService = syncService;
            SearchResults = new ObservableCollection<ApodEntry>();

            _syncService.SyncProgressChanged += SyncService_SyncProgressChanged;
            UpdateSyncStatus();

        }

        private void SyncService_SyncProgressChanged(object sender, EventArgs e)
        {

            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(UpdateSyncStatus);
        }

        private void UpdateSyncStatus()
        {
            SyncStatus = _syncService.SyncStatus;
            IsSyncing = _syncService.IsSyncing;

        }

        public async Task InitializeViewModelAsync()
        {
            IsLoading = true;
            await LoadEntriesCacheAsync();
            await ExecuteSearchAsync();
            IsLoading = false;
        }

        private async Task LoadEntriesCacheAsync()
        {
            if (_allEntriesCache == null)
            {
                System.Diagnostics.Debug.WriteLine("Loading entries cache for search...");
                await _databaseService.InitializeAsync();
                _allEntriesCache = await _databaseService.GetEntriesAsync() ?? new List<ApodEntry>();
                System.Diagnostics.Debug.WriteLine($"Loaded {_allEntriesCache.Count} entries into cache.");
            }
        }

        private void ScheduleSearch()
        {

            _debounceTimer?.Dispose();

            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();

            _debounceTimer = new Timer(async (_) => await ExecuteSearchAsync(_searchCts.Token), null, TimeSpan.FromMilliseconds(500), Timeout.InfiniteTimeSpan);
        }

        private async Task ExecuteSearchAsync(CancellationToken cancellationToken = default)
        {

            if (_allEntriesCache == null)
            {
                await LoadEntriesCacheAsync();
            }

            if (_allEntriesCache == null)
            {
                System.Diagnostics.Debug.WriteLine("Entry cache is null, cannot search.");
                SearchResults = new ObservableCollection<ApodEntry>();
                return;
            }

            IsSearching = true;
            List<ApodEntry> results;

            try
            {
                if (string.IsNullOrWhiteSpace(SearchTerm))
                {

                    results = _allEntriesCache.OrderByDescending(e => e.Date).Take(100).ToList();
                    System.Diagnostics.Debug.WriteLine($"Displaying recent {_allEntriesCache.Count} entries.");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Performing fuzzy search for: {SearchTerm}");
                    results = await _searchService.FuzzySearchAsync(SearchTerm, _allEntriesCache);
                    System.Diagnostics.Debug.WriteLine($"Fuzzy search found {results.Count} results.");
                }

                cancellationToken.ThrowIfCancellationRequested();

                Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
                {
                    SearchResults = new ObservableCollection<ApodEntry>(results);
                });
            }
            catch (OperationCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("Search operation cancelled.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during search execution: {ex.Message}");

                Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
                {
                    SearchResults = new ObservableCollection<ApodEntry>();
                });
            }
            finally
            {
                IsSearching = false;
            }
        }

        public void NavigateToDetail(ApodEntry entry)
        {
            if (entry != null)
            {
                System.Diagnostics.Debug.WriteLine($"Navigating to detail for date: {entry.Date}");

                _navigationManager.NavigateTo($"image/{entry.Date}");
            }
        }

        public void Dispose()
        {
            _debounceTimer?.Dispose();
            _searchCts?.Cancel();
            _searchCts?.Dispose();
            _syncService.SyncProgressChanged -= SyncService_SyncProgressChanged;
        }
    }
}