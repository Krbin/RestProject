using Microsoft.AspNetCore.Components;
using RestProject.Models;
using RestProject.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace RestProject.ViewModels
{
    public class MonthViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly NavigationManager _navigationManager;

        private int _year;
        public int Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        private int _month;
        public int Month
        {
            get => _month;
            set => SetProperty(ref _month, value);
        }

        private ObservableCollection<ApodEntry> _entries;
        public ObservableCollection<ApodEntry> Entries
        {
            get => _entries;
            private set => SetProperty(ref _entries, value);
        }

        private bool _isLoading = true;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public MonthViewModel(IDatabaseService databaseService, NavigationManager navigationManager)
        {
            _databaseService = databaseService;
            _navigationManager = navigationManager;
            Entries = new ObservableCollection<ApodEntry>();
        }

        public async Task LoadDataAsync()
        {
            if (Year == 0 || Month == 0) return;

            IsLoading = true;
            Entries.Clear();

            try
            {
                await _databaseService.InitializeAsync();
                var monthEntries = await _databaseService.GetEntriesForMonthAsync(Year, Month);
                System.Diagnostics.Debug.WriteLine($"Found {monthEntries.Count} entries for {Year}-{Month:D2}.");

                Entries = new ObservableCollection<ApodEntry>(monthEntries);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading month view data for {Year}-{Month:D2}: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
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
    }
}