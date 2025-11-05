using FCG_Games.Domain.Games.Entities;
using FCG_Games.Infrasctructure.Games.Mappings;
using Microsoft.EntityFrameworkCore;

namespace FCG_Games.Infrasctructure.Shared.Context
{
    public class GamesDbContext(DbContextOptions<GamesDbContext> options) : DbContext(options)
    {
        public DbSet<Game> Games { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameMap());
        }
    }
}
