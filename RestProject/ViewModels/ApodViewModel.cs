using RestProject.Database;
using RestProject.Models;
using RestProject.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RestProject.ViewModels
{
    public class ApodViewModel : INotifyPropertyChanged
    {
        private readonly ApodService _apodService;
        private readonly SqliteService _databaseService;

        private ObservableCollection<ApodModel> _apodItems;
        public ObservableCollection<ApodModel> ApodModels
        {
            get => _apodItems;
            set
            {
                _apodItems = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ApodViewModel(ApodService apodService, SqliteService databaseService)
        {
            _apodService = apodService;
            _databaseService = databaseService;
            var ApodModels = new ObservableCollection<ApodModel>();
        }

        //public async Task LoadApodDataAsync()
        //{
        //    try
        //    {
        //        string startDate = "2024-01-01";
        //        string endDate = "2024-01-31";

        //        await _apodService.FetchAndStoreApodImagesForRange(startDate, endDate);

        //        var allApodModels = await _databaseService.();

        //        ApodModels.Clear();
        //        foreach (var apod in allApodModels)
        //        {
        //            ApodModels.Add(apod);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Error loading APOD data: {ex.Message}");
        //    }
        //}
    }
}
