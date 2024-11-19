using RestProject.Models;
using RestProject.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RestProject.ViewModels
{
    public class ApodViewModel : INotifyPropertyChanged
    {
        private readonly ISqliteService _sqliteService;
        private ApodModel _apodModel;
        private ObservableCollection<ApodModel> _apodItems;

        public ApodModel ApodModel
        {
            get => _apodModel;
            set
            {
                _apodModel = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ApodModel> ApodItems
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

        public ApodViewModel(ISqliteService sqliteService)
        {
            _sqliteService = sqliteService;
            ApodModel = new ApodModel();
            ApodItems = new ObservableCollection<ApodModel>();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var apodModels = await _sqliteService.GetAllDataAsync<ApodModel>();
                foreach (var model in apodModels)
                {
                    ApodItems.Add(model);
                }
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
            }
        }
    }
}
