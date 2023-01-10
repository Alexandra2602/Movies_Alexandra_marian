using Microsoft.EntityFrameworkCore;
using Movies_Alexandra_marian.Models;

namespace Movies_Alexandra_marian.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Distribution> Distributions { get; set; }
        public DbSet<DistributedMovie> DistributedMovies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<History>().ToTable("History");
            modelBuilder.Entity<Movie>().ToTable("Movie");
            modelBuilder.Entity<Director>().ToTable("Director");
            modelBuilder.Entity<Distribution>().ToTable("Distribution");
            modelBuilder.Entity<DistributedMovie>().ToTable("DistributedMovie");
            modelBuilder.Entity<DistributedMovie>()
            .HasKey(c => new { c.MovieID, c.DistributionID });//configureaza cheia primara compusa

        }
    }
}
