
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Theatre> Theatres { get; set; } = null!;
        public DbSet<Screen> Screens { get; set; } = null!;
        public DbSet<Seat> Seats { get; set; } = null!;
    }
}
