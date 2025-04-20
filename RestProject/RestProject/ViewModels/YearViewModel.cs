using Microsoft.AspNetCore.Components;
using RestProject.Models;
using RestProject.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace RestProject.ViewModels
{

    public class MonthGroup
    {
        public int Month { get; set; }
        public ApodEntry PreviewEntry { get; set; }
    }

    public class YearViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly NavigationManager _navigationManager;

        private int _year;
        public int Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        private ObservableCollection<MonthGroup> _monthGroups;
        public ObservableCollection<MonthGroup> MonthGroups
        {
            get => _monthGroups;
            private set => SetProperty(ref _monthGroups, value);
        }

        private bool _isLoading = true;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public YearViewModel(IDatabaseService databaseService, NavigationManager navigationManager)
        {
            _databaseService = databaseService;
            _navigationManager = navigationManager;
            MonthGroups = new ObservableCollection<MonthGroup>();
        }

        public async Task LoadDataAsync()
        {
            if (Year == 0) return;

            IsLoading = true;
            MonthGroups.Clear();

            try
            {
                await _databaseService.InitializeAsync();
                var groups = await _databaseService.GetEntriesGroupedByMonthAsync(Year);
                System.Diagnostics.Debug.WriteLine($"Found {groups.Count} month groups for year {Year}.");

                var monthGroupList = new List<MonthGroup>();
                foreach (var group in groups.OrderBy(g => g.Key))
                {
                    var preview = await _databaseService.GetLastEntryForMonthAsync(Year, group.Key);
                    if (group.Any() && preview != null)
                    {
                        monthGroupList.Add(new MonthGroup { Month = group.Key, PreviewEntry = preview });
                    }
                    else if (group.Any())
                    {
                        monthGroupList.Add(new MonthGroup { Month = group.Key, PreviewEntry = null });
                        System.Diagnostics.Debug.WriteLine($"Could not find preview for month {group.Key} in year {Year}");
                    }
                }
                MonthGroups = new ObservableCollection<MonthGroup>(monthGroupList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading year view data for {Year}: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void NavigateToMonth((int Year, int Month) yearMonth)
        {
            System.Diagnostics.Debug.WriteLine($"Navigating to month: {yearMonth.Month}, year: {yearMonth.Year}");

            _navigationManager.NavigateTo($"month/{yearMonth.Year}/{yearMonth.Month}");
        }
    }
}