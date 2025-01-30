using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public async Task UpdateAsync(int videoGameId, VideoGame newGame)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                try
                {
                    var videoGame = _dataContext.VideoGames.Single(x => x.Id == videoGameId);
                    var publisher = await GetOrCreatePublisherAsync(newGame.Publisher.Name);
                    videoGame.Publisher = publisher;

                    await _dataContext.GenreVideoGame.Where(x => x.VideoGamesId == videoGameId).ExecuteDeleteAsync();
                    videoGame.Genres = new List<Genre>();
                    if (newGame.Genres != null && newGame.Genres.Any())
                    {
                        for (var i = 0; i < newGame.Genres.Count; i++)
                        {
                            var genre = newGame.Genres[i];
                            var dbGenre = await GetOrCreateGenreAsync(genre.Name);
                            videoGame.Genres.Add(dbGenre);
                        }
                    }

                    videoGame.Name = newGame.Name;
                    videoGame.Details.Description = newGame.Details.Description;
                    videoGame.Details.ReleaseDate = newGame.Details.ReleaseDate;
                    
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

        public async Task DeleteAsync(int videoGameId)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                try
                {
                    var videoGame = _dataContext.VideoGames.Single(x => x.Id == videoGameId);
                    _dataContext.VideoGames.Remove(videoGame);
                    await _dataContext.SaveChangesAsync();

                    await _dataContext.Genres.Where(x => !x.VideoGames.Any()).ExecuteDeleteAsync();
                    await _dataContext.Publishers.Where(p => !_dataContext.VideoGames.Any(v => v.PublisherId == p.Id))
                        .ExecuteDeleteAsync();
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
