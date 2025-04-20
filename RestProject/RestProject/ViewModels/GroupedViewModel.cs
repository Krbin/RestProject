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

    public class YearGroup
    {
        public int Year { get; set; }
        public ApodEntry PreviewEntry { get; set; }
    }

    public class GroupedViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly NavigationManager _navigationManager;

        private ObservableCollection<YearGroup> _yearGroups;
        public ObservableCollection<YearGroup> YearGroups
        {
            get => _yearGroups;
            private set => SetProperty(ref _yearGroups, value);
        }

        private bool _isLoading = true;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public GroupedViewModel(IDatabaseService databaseService, NavigationManager navigationManager)
        {
            _databaseService = databaseService;
            _navigationManager = navigationManager;
            YearGroups = new ObservableCollection<YearGroup>();
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            YearGroups.Clear();

            try
            {
                await _databaseService.InitializeAsync();
                var groups = await _databaseService.GetEntriesGroupedByYearAsync();

                System.Diagnostics.Debug.WriteLine($"Found {groups.Count} year groups.");

                var yearGroupList = new List<YearGroup>();
                foreach (var group in groups.OrderByDescending(g => g.Key))
                {
                    var preview = await _databaseService.GetLastEntryForYearAsync(group.Key);

                    if (group.Any() && preview != null)
                    {
                        yearGroupList.Add(new YearGroup { Year = group.Key, PreviewEntry = preview });
                    }
                    else if (group.Any())
                    {

                        yearGroupList.Add(new YearGroup { Year = group.Key, PreviewEntry = null });
                        System.Diagnostics.Debug.WriteLine($"Could not find preview for year {group.Key}");
                    }
                }
                YearGroups = new ObservableCollection<YearGroup>(yearGroupList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading grouped view data: {ex.Message}");

            }
            finally
            {
                IsLoading = false;
            }
        }

        public void NavigateToYear(int year)
        {
            System.Diagnostics.Debug.WriteLine($"Navigating to year: {year}");

            _navigationManager.NavigateTo($"year/{year}");
        }
    }
}