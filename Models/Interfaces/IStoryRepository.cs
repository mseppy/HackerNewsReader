using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.Interfaces
{
    public interface IStoryRepository
    {
        Task<List<string>> GetTopStoriesAsync(int count);
    }
}
