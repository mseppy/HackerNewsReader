using Microsoft.Extensions.Logging;
using Models;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class StoryService : IStoryService
    {
        private readonly ILogger<StoryService> _logger;
        private readonly IStoryRepository storyRepo;

        public int DefaultItemsPerPage { get; set; }

        public StoryService(ILogger<StoryService> logger, IStoryRepository storyRepository)
        {
            _logger = logger;
            storyRepo = storyRepository;

        }

        public async Task<List<Story>> GetNewStoriesAsync(int count)
        {
            var max = (count < 1) ? DefaultItemsPerPage : count;

            var result = new List<Story>();
            try
            {
                result = await storyRepo.GetNewStoriesAsync(max);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetNewStories() failed, count = {count}");
            }
            return result;
        }

        public async Task<List<Story>> GetTopStoriesAsync(int count)
        {
            var max = (count < 1) ? DefaultItemsPerPage : count;

            var result = new List<Story>();
            try
            {
                result = await storyRepo.GetTopStoriesAsync(max);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetTopStories() failed, count = {count}");
            }
            return result;
        }

        public async Task<List<Story>> GetBestStoriesAsync(int count)
        {
            var max = (count < 1) ? DefaultItemsPerPage : count;

            var result = new List<Story>();
            try
            {
                result = await storyRepo.GetBestStoriesAsync(max);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetBestStories() failed, count = {count}");
            }
            return result;
        }
    }
}
