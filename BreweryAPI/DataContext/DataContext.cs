using BreweryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.DataContext
{
     public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<Beer> Beers { get; set; }
        public DbSet<BreweryStock> BreweryBeers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
