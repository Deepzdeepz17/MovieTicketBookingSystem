
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly ApplicationDbContext _context;

        public ShowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Show>> GetAllShowsAsync()
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .ToListAsync();
        }

        public async Task<Show?> GetShowByIdAsync(int id)
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .FirstOrDefaultAsync(s => s.ShowId == id);
        }

        public async Task<Show> AddShowAsync(Show show)
        {
            using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Shows.AddAsync(show);
                await _context.SaveChangesAsync();

                var seats = await _context.Seats
                    .Where(s => s.ScreenId == show.ScreenId)
                    .ToListAsync();

                if (!seats.Any())
                {
                    throw new InvalidOperationException(
                        "No seats are available for this screen.");
                }

                var showSeats = seats.Select(seat => new ShowSeat
                {
                    ShowId = show.ShowId,
                    SeatId = seat.SeatId,
                    Status = "Available"
                }).ToList();

                await _context.ShowSeats.AddRangeAsync(showSeats);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return show;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Show?> UpdateShowAsync(int id, Show show)
        {
            var existingShow = await _context.Shows.FindAsync(id);

            if (existingShow == null)
                return null;

            existingShow.MovieId = show.MovieId;
            existingShow.ScreenId = show.ScreenId;
            existingShow.StartTime = show.StartTime;
            existingShow.EndTime = show.EndTime;
            existingShow.TicketPrice = show.TicketPrice;

            await _context.SaveChangesAsync();

            return existingShow;
        }

        public async Task<bool> DeleteShowAsync(int id)
        {
            var show = await _context.Shows.FindAsync(id);

            if (show == null)
                return false;

            _context.Shows.Remove(show);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
