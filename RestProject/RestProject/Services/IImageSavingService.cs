using System.Threading.Tasks;

namespace RestProject.Services
{
    public interface IImageSavingService
    {
        Task<bool> SaveImageAsync(string imageUrl, string title);
    }
}