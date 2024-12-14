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
                .FirstOrDefaultAsync(x => x.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task AddAsync(VideoGame videoGame)
        {
            using (var transaction =  _dataContext.Database.BeginTransaction())
            {
                try
                {
                    var publisherId = await GetOrCreatePublisherAsync(videoGame.Publisher?.Name);
                    videoGame.PublisherId = publisherId;

                    _dataContext.Add(videoGame);
                    await _dataContext.SaveChangesAsync();

                    _dataContext.Add(new VideoGameDetail
                    {
                        VideoGameId = videoGame.Id,
                        Description = videoGame.Details?.Description,
                        ReleaseDate = videoGame.Details?.ReleaseDate,
                    });

                    if (videoGame.Genres != null && videoGame.Genres.Any())
                    {
                        foreach (var genre in videoGame.Genres)
                        {
                            var genreId = await GetOrCreateGenreAsync(genre.Name);
                            _dataContext.Add(new GenreVideoGame
                            {
                                GenresId = genreId,
                                VideoGamesId = videoGame.Id
                            });

                        }
                    }

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

        private async Task<int> GetOrCreatePublisherAsync(string publisherName)
        {
            var publisher = await _dataContext.Publishers.FirstOrDefaultAsync(p =>
                p.Name.Equals(publisherName, StringComparison.InvariantCultureIgnoreCase));
            if (publisher == null)
            {
                publisher = new Publisher { Name = publisherName };
                _dataContext.Add(publisher);
                await _dataContext.SaveChangesAsync();
            }

            return publisher.Id;
        }

        private async Task<int> GetOrCreateGenreAsync(string genreName)
        {
            var genre = await _dataContext.Genres.FirstOrDefaultAsync(g =>
                g.Name.Equals(genreName, StringComparison.InvariantCultureIgnoreCase));
            if (genre == null)
            {
                genre = new Genre { Name = genreName };
                _dataContext.Add(genre);
                await _dataContext.SaveChangesAsync();
            }

            return genre.Id;
        }

        
    }
}
