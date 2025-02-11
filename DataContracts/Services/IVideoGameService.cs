using DataContracts.Models;

namespace DataContracts.Services
{
    public interface IVideoGameService
    {
        Task<IEnumerable<VideoGame>> GetAllAsync();

        Task<VideoGame> GetByCodeAsync(string code);

        Task AddAsync(VideoGame videoGame);

        Task UpdateAsync(int videoGameId, VideoGame newGame);

        Task DeleteAsync(int videoGameId);
    }
}
