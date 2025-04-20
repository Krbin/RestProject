using System.Threading.Tasks;

namespace RestProject.Services
{
    public interface ISharingService
    {
        Task ShareImageAsync(string imageUrl, string title);
        Task ShareTextAsync(string text, string title);
    }
}