using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestProject.Services
{
    public class ImageSavingService : IImageSavingService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<bool> SaveImageAsync(string imageUrl, string title)
        {
            if (string.IsNullOrEmpty(imageUrl)) return false;

            string tempPath = null;
            string destinationPath = null;
            try
            {
                System.Diagnostics.Debug.WriteLine($"Attempting to download image for saving: {imageUrl}");
                tempPath = await DownloadImageAsync(imageUrl, title);

                if (string.IsNullOrEmpty(tempPath))
                {
                    System.Diagnostics.Debug.WriteLine("Download failed, cannot save image.");
                    await ShowToast("Failed to download image.");
                    return false;
                }

                string targetFileName = Path.GetFileName(tempPath);
                destinationPath = Path.Combine(FileSystem.AppDataDirectory, targetFileName);

                File.Move(tempPath, destinationPath);

                System.Diagnostics.Debug.WriteLine($"Image successfully saved to AppData: {destinationPath}");

                await ShowToast($"Image saved: {targetFileName}");

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving image: {ex.Message}");
                await ShowToast($"Error saving image: {ex.Message}");

                if (!string.IsNullOrEmpty(destinationPath) && File.Exists(destinationPath))
                {
                    try { File.Delete(destinationPath); } catch { }
                }
                return false;
            }
            finally
            {

                if (!string.IsNullOrEmpty(tempPath) && File.Exists(tempPath))
                {
                    try { File.Delete(tempPath); } catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Failed to delete temp save file: {ex.Message}"); }
                }
            }
        }

        private async Task<string> DownloadImageAsync(string imageUrl, string title)
        {
            try
            {
                using var response = await _httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";
                var stream = await response.Content.ReadAsStreamAsync();

                string ext = InferExtension(contentType);
                string baseName = SanitizeFileName(title ?? "apod");
                string fileName = $"{baseName}_{Path.GetRandomFileName()}{ext}";
                string tempPath = Path.Combine(FileSystem.CacheDirectory, fileName);

                using (var fileStream = File.Create(tempPath))
                {
                    await stream.CopyToAsync(fileStream);
                }
                return tempPath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error downloading image: {ex.Message}");
                return null;
            }
        }

        private string SanitizeFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "apod_image";
            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            string sanitized = name;
            foreach (char c in invalidChars) { sanitized = sanitized.Replace(c.ToString(), ""); }
            sanitized = sanitized.Replace(" ", "_");
            const int MaxLength = 50;
            if (sanitized.Length > MaxLength) sanitized = sanitized.Substring(0, MaxLength);
            return sanitized;
        }

        private string InferExtension(string contentType) => contentType.ToLowerInvariant() switch
        {
            "image/png" => ".png",
            "image/jpeg" => ".jpg",
            "image/gif" => ".gif",
            "image/bmp" => ".bmp",
            "image/webp" => ".webp",
            _ => ".jpg"
        };

        private async Task ShowToast(string message)
        {

            try
            {

                var toast = CommunityToolkit.Maui.Alerts.Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short);
                await toast.Show();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to show toast: {ex.Message}. Message: {message}");
            }

        }
    }
}