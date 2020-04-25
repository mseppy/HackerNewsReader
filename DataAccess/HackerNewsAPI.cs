using Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess
{
    public class HackerNewsAPI : IRepository
    {
        readonly HttpClient httpClient;


        public HackerNewsAPI(HttpClient client)
        {
            httpClient = client;
        }


        async Task<int> GetMaxId()
        {
            var response = await httpClient.GetAsync("maxitem.json");
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<int>(responseStream);
        }
    }
}
