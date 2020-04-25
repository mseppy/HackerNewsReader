using Microsoft.Extensions.Logging;
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

        public int DefaultItemsPerPage {get; set;}
        public StoryService(ILogger<StoryService> logger, IStoryRepository storyRepository)
        {
            _logger = logger;
            storyRepo = storyRepository;

            DefaultItemsPerPage = 100;
        }

        public async Task<object> GetTopStoriesAsync(int? count)
        {
            var max = count ?? DefaultItemsPerPage;

            var result = new List<string>();
            try
            {
                result = await storyRepo.GetTopStoriesAsync(max);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"GetTopStories() failed, count = {count}");
            }
            return result;
        }

    }
}
