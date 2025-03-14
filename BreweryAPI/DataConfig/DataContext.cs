using BreweryAPI.Models;
using BreweryAPI.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<Beer> Beers { get; set; }
        public DbSet<BreweryStock> BreweryBeers { get; set; }
        public DbSet<Wholesaler> Wholesalers { get; set; }
        public DbSet<WholesalerStock> WholesalerStocks { get; set; }
        public DbSet<User> Users{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring the relationship between Brewery and Beer
            modelBuilder.Entity<Beer>()
                .HasOne(b => b.Brewery) // Beer has one Brewery
                .WithMany(br => br.Beers) // Brewery can have many Beers
                .HasForeignKey(b => b.BreweryId); // Foreign key in Beer table

             modelBuilder.Entity<BreweryStock>().ToTable("BreweryStocks");

            // Configuring the relationship between BreweryStock and Brewery
            modelBuilder.Entity<BreweryStock>()
                .HasOne(bs => bs.Brewery) // BreweryStock has one Brewery
                .WithMany(br => br.BreweryStocks) // Brewery can have many BreweryStocks
                .HasForeignKey(bs => bs.BreweryId) // Foreign key in BreweryStock table
                .OnDelete(DeleteBehavior.NoAction); 

            // Configuring the relationship between BreweryStock and Beer
            modelBuilder.Entity<BreweryStock>()
                .HasOne(bs => bs.Beer) // BreweryStock has one Beer
                .WithMany(b => b.BreweryStocks) // Beer can have many BreweryStocks
                .HasForeignKey(bs => bs.BeerId) // Foreign key in BreweryStock table
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<WholesalerStock>()
                .HasOne(ws => ws.Wholesaler)
                .WithMany(w => w.WholesalerStocks) 
                .HasForeignKey(ws => ws.WholesalerId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<WholesalerStock>()
                .HasOne(ws => ws.Beer)
                .WithMany(b => b.WholesalerStocks) 
                .HasForeignKey(ws => ws.BeerId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<BreweryStock>()
                .Property(bs => bs.Amount)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.UserName)
                .IsRequired();
        }
    }
}
