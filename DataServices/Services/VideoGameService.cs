using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using DataContracts.Models;
using DataContracts.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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
            return await _dataContext.VideoGames.Include(v => v.Details)
                                                .Include(v => v.Publisher)
                                                .Include(v => v.Genres)
                                                .ToListAsync();
        }

        public async Task<VideoGame> GetByCodeAsync(string code)
        {
            return await _dataContext.VideoGames
                .Include(v => v.Details)
                .Include(v => v.Publisher)
                .Include(v => v.Genres)
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task AddAsync(VideoGame videoGame)
        {
            using (var transaction =  _dataContext.Database.BeginTransaction())
            {
                try
                {
                    var publisher = await GetOrCreatePublisherAsync(videoGame.Publisher.Name);
                    videoGame.Publisher = publisher;

                    if (videoGame.Genres != null && videoGame.Genres.Any())
                    {
                        for (var i = 0; i < videoGame.Genres.Count; i++)
                        {
                            var genre = videoGame.Genres[i];
                            var dbGenre = await GetOrCreateGenreAsync(genre.Name);
                            videoGame.Genres[i] = dbGenre;
                        }
                    }

                    _dataContext.VideoGames.Add(videoGame);
                    await _dataContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private async Task<Publisher> GetOrCreatePublisherAsync(string publisherName)
        {
            var publisher = await _dataContext.Publishers.FirstOrDefaultAsync(p =>
                p.Name == publisherName);
            if (publisher == null)
            {
                publisher = new Publisher { Name = publisherName };
                _dataContext.Publishers.Add(publisher);
                await _dataContext.SaveChangesAsync();
            }

            return publisher;
        }

        private async Task<Genre> GetOrCreateGenreAsync(string genreName)
        {
            var genre = await _dataContext.Genres.FirstOrDefaultAsync(g =>
                g.Name == genreName);
            if (genre == null)
            {
                genre = new Genre { Name = genreName };
                _dataContext.Genres.Add(genre);
                await _dataContext.SaveChangesAsync();
            }

            return genre;
        }
    }
}
