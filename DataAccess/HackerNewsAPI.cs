using Models.Interfaces;
using System.Collections.Generic;
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
            httpClient = this.clientFactory.CreateClient();
        }


        public async Task<List<string>> GetTopStoriesAsync(int count = 50)
        {
            return await Get<List<string>>("topstories.json");
        }
        public async Task<int> GetMaxIdAsync()
        {
            return await Get<int>("maxitem.json");
        }

        private async Task<TValue> Get<TValue>(string endpoint)
        {
            var response = await httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<TValue>(responseStream);
        }
    }
}
