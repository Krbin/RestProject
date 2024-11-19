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
        private readonly ISqliteService _sqliteService;
        private ApodModel _apodModel;
        private ObservableCollection<ApodModel> _apodItems;
        public ApodModel apodModel
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
            apodModel = new ApodModel();
            ApodItems = new ObservableCollection<ApodModel>();
        }
        private async Task LoadDataAsync()
        {
            var apodModels = await _sqliteService.GetAllDataAsync<ApodModel>();
            foreach (var model in apodModels)
            {
                ApodItems.Add(model);
            }
        }
    }
}
