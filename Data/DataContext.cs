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

        public DbSet<Publisher> Publishers { get; set; }
    }
}