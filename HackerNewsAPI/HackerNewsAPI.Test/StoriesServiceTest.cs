using HackerNewsAPI.Controllers;
using HackerNewsAPI.Models;
using HackerNewsAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNewsAPI.Test
{
    [TestClass]
    public class StoriesServiceTest
    {
        [TestMethod]
        public async Task GetBestStoriesTest()
        {
            // Arrange
            var mockService = new Mock<IStoriesServices>();
            mockService.Setup(s => s.GetBestStoriesIdsAsync(It.IsAny<int>())).ReturnsAsync(new List<int> { 1, 2, 3 });
            mockService.Setup(s => s.GetStoryDetailsAsync(It.IsAny<int>())).ReturnsAsync(new Models.Stories
            {
                Title = "test",
                By = "test",
                Descendants = 10,
                Score = 10,
                Time = 10,
                Url = "test",
            });
            List<StoriesViewModel> stories = new List<StoriesViewModel>();
            stories.Add(new Models.StoriesViewModel
            {
                Title = "test",
                PostedBy = "test",
                CommentCount = 10,
                Score = 10,
                Time = DateTime.Now,
                Uri = "test",
            });
            mockService.Setup(s => s.GetStoriesAsync(It.IsAny<int>())).ReturnsAsync(stories);

            var controller = new StoriesController(mockService.Object);

            // Act
            var result = await controller.GetBestStories(3);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Value.Count);
        }
    }
}
