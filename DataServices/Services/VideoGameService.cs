using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using DataContracts.Models;
using DataContracts.Services;
using Microsoft.EntityFrameworkCore;

namespace DataServices.Services
{
    public class VideoGameService : IVideoGameService
    {
        private readonly DataContext _dataContext;

        public VideoGameService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<VideoGame>> GetAllAsync()
        {
            return await _dataContext.VideoGames.ToListAsync();
        }

        public async Task<VideoGame> GetByCodeAsync(string code)
        {
            return await _dataContext.VideoGames.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task AddAsync(VideoGame videoGame)
        {
            _dataContext.VideoGames.Add(videoGame);
            await _dataContext.SaveChangesAsync();
        }
    }
}
