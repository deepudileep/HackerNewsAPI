﻿using HackerNewsAPI.Models;
using HackerNewsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsAPI.Controllers
{
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly IStoriesServices storiesServices;

        public StoriesController(IStoriesServices storiesServices)
        {
            this.storiesServices = storiesServices;
        }

        
        [HttpGet("story/{n}")]
        public async Task<ActionResult<List<StoriesViewModel>>> GetBestStories(int n)
        {
            return await this.storiesServices.GetStoriesAsync(n);
        }
    }
}
