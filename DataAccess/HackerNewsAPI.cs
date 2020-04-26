using Models;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess
{
    public class HackerNewsAPI : IStoryRepository
    {
        readonly HttpClient httpClient;
        IHttpClientFactory clientFactory;

        public HackerNewsAPI(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            httpClient = this.clientFactory.CreateClient("hackerNews");
        }

        public async Task<List<Story>> GetNewStoriesAsync(int count)
        {
            if (count < 0) { throw new InvalidOperationException($"Negative numbers are invalid. (count = {count}"); }

            if (count == 0) { return new List<Story>(); }

            return await Get("newstories", count);
        }

        public async Task<List<Story>> GetTopStoriesAsync(int count)
        {
            if (count < 0) { throw new InvalidOperationException($"Negative numbers are invalid. (count = {count}"); }

            if (count == 0) { return new List<Story>(); }

            return await Get("topstories", count);
        }

        public async Task<List<Story>> GetBestStoriesAsync(int count)
        {
            if (count < 0) { throw new InvalidOperationException($"Negative numbers are invalid. (count = {count}"); }

            if (count == 0) { return new List<Story>(); }

            return await Get("beststories", count);
        }

        private async Task<List<Story>> Get(string endpoint, int count)
        {
            var allIDs = await HttpGet<int[]>(endpoint);
            var takingIDs = allIDs.Take(count);

            var stories = new List<Story>();
            foreach (var id in takingIDs)
            {
                var story = Map(await HttpGet<HackerNewsStory>($"item/{id}"));
                if (story.Type == "job") continue;
                stories.Add(story);
            }
            return stories;
        }

        private async Task<TValue> HttpGet<TValue>(string endpoint)
        {
            var response = await httpClient.GetAsync(endpoint + ".json");
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<TValue>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IgnoreNullValues = true });
        }

        private Story Map(HackerNewsStory story)
        {
            return new Story
            {
                Title = story.Title,
                Url = story.Url,
                UserName = story.By,
                Time = MapTimeStampToDateTime(story.Time)
            };
        }

        // https://en.wikipedia.org/wiki/Unix_time is the number of seconds since Jan 1, 1970 GMT
        private DateTimeOffset MapTimeStampToDateTime(double unixTime)
        {
            var epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, new TimeSpan());
            return epoch.AddSeconds(unixTime);
        }
    }
}
