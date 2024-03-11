using HackerNewsAPI.Controllers;
using HackerNewsAPI.Models;

namespace HackerNewsAPI.Services
{
    public interface IStoriesServices
    {
        Task<List<StoriesViewModel>> GetStoriesAsync(int n);
        Task<List<int>> GetBestStoriesIdsAsync(int n);
        Task<Stories> GetStoryDetailsAsync(int storyId);
    }
}
