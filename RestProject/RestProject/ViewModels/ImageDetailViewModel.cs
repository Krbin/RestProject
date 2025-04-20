using Microsoft.AspNetCore.Components;
using RestProject.Models;
using RestProject.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System;

namespace RestProject.ViewModels
{
    public class ImageDetailViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly ISharingService _sharingService;
        private readonly IImageSavingService _imageSavingService;
        private readonly NavigationManager _navigationManager;

        private string _date;
        public string Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private ApodEntry _entry;
        public ApodEntry Entry
        {
            get => _entry;
            private set => SetProperty(ref _entry, value);
        }

        private bool _isLoading = true;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                {

                    (SaveCommand as Command)?.ChangeCanExecute();
                    (ShareCommand as Command)?.ChangeCanExecute();
                }
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand ShareCommand { get; }

        public ImageDetailViewModel(IDatabaseService databaseService, ISharingService sharingService, IImageSavingService imageSavingService, NavigationManager navigationManager)
        {
            _databaseService = databaseService;
            _sharingService = sharingService;
            _imageSavingService = imageSavingService;
            _navigationManager = navigationManager;

            SaveCommand = new Command(async () => await ExecuteSaveAsync(), () => Entry != null && Entry.IsImage && !IsBusy);
            ShareCommand = new Command(async () => await ExecuteShareAsync(), () => Entry != null && !IsBusy);
        }

        public async Task LoadDataAsync()
        {
            if (string.IsNullOrEmpty(Date)) return;

            IsLoading = true;
            StatusMessage = string.Empty;
            Entry = null;

            try
            {
                await _databaseService.InitializeAsync();
                Entry = await _databaseService.GetEntryByDateAsync(Date);
                System.Diagnostics.Debug.WriteLine(Entry == null ? $"Entry not found for date {Date}" : $"Entry loaded for date {Date}");

                (SaveCommand as Command)?.ChangeCanExecute();
                (ShareCommand as Command)?.ChangeCanExecute();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading image detail for {Date}: {ex.Message}");
                StatusMessage = "Error loading details.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ExecuteSaveAsync()
        {
            if (IsBusy || Entry == null || !Entry.IsImage) return;

            IsBusy = true;
            StatusMessage = "Saving...";
            System.Diagnostics.Debug.WriteLine($"Executing Save for: {Entry.Url}");

            try
            {
                bool success = await _imageSavingService.SaveImageAsync(Entry.Url, Entry.Title);
                StatusMessage = success ? "Image saved successfully!" : "Failed to save image.";
                System.Diagnostics.Debug.WriteLine($"Save result: {success}");

                if (success) await Task.Delay(3000); StatusMessage = string.Empty;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving image: {ex.Message}");
                StatusMessage = "An error occurred while saving.";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteShareAsync()
        {
            if (IsBusy || Entry == null) return;

            IsBusy = true;
            StatusMessage = "Preparing share...";
            System.Diagnostics.Debug.WriteLine($"Executing Share for: {Entry.Title}");

            try
            {
                if (Entry.IsImage && !string.IsNullOrEmpty(Entry.Url))
                {
                    await _sharingService.ShareImageAsync(Entry.Url, Entry.Title);
                }
                else
                {
                    string textToShare = $"{Entry.Title} ({Entry.Date})\n{Entry.Explanation}\nLink: {Entry.Url}";
                    await _sharingService.ShareTextAsync(textToShare, Entry.Title);
                }
                StatusMessage = "Shared!";
                await Task.Delay(2000); StatusMessage = string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sharing item: {ex.Message}");
                StatusMessage = "An error occurred while sharing.";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}