
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
        public DbSet<Show> Shows { get; set; } = null!;
        public DbSet<ShowSeat> ShowSeats { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<BookingSeat> BookingSeats { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ShowSeat relationships
            modelBuilder.Entity<ShowSeat>()
                .HasOne(ss => ss.Show)
                .WithMany(s => s.ShowSeats)
                .HasForeignKey(ss => ss.ShowId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ShowSeat>()
                .HasOne(ss => ss.Seat)
                .WithMany()
                .HasForeignKey(ss => ss.SeatId)
                .OnDelete(DeleteBehavior.NoAction);

            // Booking relationships
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Show)
                .WithMany()
                .HasForeignKey(b => b.ShowId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.BookingReference)
                .IsUnique();

            // BookingSeat relationships
            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Seat)
                .WithMany()
                .HasForeignKey(bs => bs.SeatId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
