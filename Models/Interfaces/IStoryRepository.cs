using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.Interfaces
{
    public interface IStoryRepository
    {
        Task<List<Story>> GetNewStoriesAsync(int count);
        Task<List<Story>> GetTopStoriesAsync(int count);
        Task<List<Story>> GetBestStoriesAsync(int count);
    }
}
