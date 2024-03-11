using HackerNewsAPI.Controllers;
using HackerNewsAPI.Models;

namespace HackerNewsAPI.Services;
public class StoriesServices : IStoriesServices
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;
    public StoriesServices(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.configuration = configuration;
    }

    public async Task<List<StoriesViewModel>> GetStoriesAsync(int n)
    {
        var ids = await GetBestStoriesIdsAsync(n);
        var stories = new List<StoriesViewModel>();
        foreach (var id in ids)
        {
            var story = await GetStoryDetailsAsync(id);
            if (story != null)
            {
                stories.Add(new StoriesViewModel
                {
                    CommentCount = story.Descendants,
                    PostedBy = story.By,
                    Score = story.Score,
                    Time = ConvertToDateTime(story.Time),
                    Title = story.Title,
                    Uri = story.Url
                });
            }
        }
        return stories.OrderByDescending(s => s.Score).ToList();
    }

    public async Task<List<int>> GetBestStoriesIdsAsync(int n)
    {
        var storiesIds = new List<int>();
        string bestStoriesUrl = this.configuration["HackerNewsUrls:BestStoriesUrl"];
        var response = await this.httpClient.GetAsync(bestStoriesUrl);
        response.EnsureSuccessStatusCode();
        storiesIds = await response.Content.ReadFromJsonAsync<List<int>>();
        if (storiesIds is not null && storiesIds.Count > 0)
            storiesIds = storiesIds.Take(n).ToList();
        return storiesIds;
    }

    public async Task<Stories> GetStoryDetailsAsync(int storyId)
    {
        string storyDetailsUrl = string.Format(this.configuration["HackerNewsUrls:StoryDetailsUrl"], storyId);
        var response = await this.httpClient.GetAsync(storyDetailsUrl);
        response.EnsureSuccessStatusCode();
        var stories = await response.Content.ReadFromJsonAsync<Stories>();
        return stories;
    }

    private DateTime ConvertToDateTime(long time)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(time);
        return dateTimeOffset.DateTime;
    }
}
