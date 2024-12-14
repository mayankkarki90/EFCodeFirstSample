using DataContracts.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<VideoGame> VideoGames { get; set; }

        public DbSet<VideoGameDetail> VideoGamesDetails { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<GenreVideoGame> GenreVideoGame { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VideoGame>()
                .HasMany(e => e.Genres)
                .WithMany(e => e.VideoGames)
                .UsingEntity<GenreVideoGame>().HasKey(j => new { j.GenresId, j.VideoGamesId });


            //l => l.HasOne<Genre>().WithMany().HasForeignKey("FK_GenreVideoGame_Genres_GenresId"),
            //r => r.HasOne<VideoGame>().WithMany().HasForeignKey("FK_GenreVideoGame_VideoGames_VideoGamesId"),
            //j => j.HasKey("GenresId", "VideoGamesId")
        }
    }
}