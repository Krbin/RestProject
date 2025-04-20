using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestProject.Services
{
    public class SharingService : ISharingService
    {

        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task ShareTextAsync(string text, string title)
        {
            try
            {
                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = text,
                    Title = title ?? "Share APOD Info"
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sharing text: {ex.Message}");

            }
        }

        public async Task ShareImageAsync(string imageUrl, string title)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string localPath = null;
            try
            {
                System.Diagnostics.Debug.WriteLine($"Attempting to download image for sharing: {imageUrl}");
                localPath = await DownloadImageAsync(imageUrl, title);

                if (!string.IsNullOrEmpty(localPath))
                {
                    System.Diagnostics.Debug.WriteLine($"Image downloaded to temporary path: {localPath}");
                    await Share.Default.RequestAsync(new ShareFileRequest
                    {
                        Title = title ?? "Share APOD Image",
                        File = new ShareFile(localPath, InferMimeType(localPath))
                    });
                    System.Diagnostics.Debug.WriteLine($"Sharing file request sent for: {localPath}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Download failed, falling back to sharing URL.");

                    await ShareTextAsync($"Check out this NASA APOD image: {imageUrl}", title);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during image sharing process: {ex.Message}");

                await ShareTextAsync($"Check out this NASA APOD image ({title}): {imageUrl}", "Share APOD Link");
            }
            finally
            {

                if (!string.IsNullOrEmpty(localPath) && File.Exists(localPath))
                {
                    try { File.Delete(localPath); } catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Failed to delete temp share file: {ex.Message}"); }
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
                System.Diagnostics.Debug.WriteLine($"Image successfully downloaded to: {tempPath}");
                return tempPath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error downloading image for sharing/saving: {ex.Message}");
                return null;
            }
        }

        private string SanitizeFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "apod_image";

            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            string sanitized = name;
            foreach (char c in invalidChars)
            {
                sanitized = sanitized.Replace(c.ToString(), "");
            }
            sanitized = sanitized.Replace(" ", "_");

            const int MaxLength = 50;
            if (sanitized.Length > MaxLength)
                sanitized = sanitized.Substring(0, MaxLength);
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

        private string InferMimeType(string filePath) => Path.GetExtension(filePath).ToLowerInvariant() switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }
}