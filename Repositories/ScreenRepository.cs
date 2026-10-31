using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories
{
    public class ScreenRepository : IScreenRepository
    {
        private readonly ApplicationDbContext _context;

        public ScreenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all screens
        public async Task<IEnumerable<Screen>> GetAllScreensAsync()
        {
            return await _context.Screens.ToListAsync();
        }

        // Get screen by ID
        public async Task<Screen?> GetScreenByIdAsync(int id)
        {
            return await _context.Screens.FindAsync(id);
        }

        // Add a new screen
        public async Task<Screen> AddScreenAsync(Screen screen)
        {
            await _context.Screens.AddAsync(screen);
            await _context.SaveChangesAsync();

            return screen;
        }

        // Update screen
        public async Task<Screen?> UpdateScreenAsync(int id, Screen screen)
        {
            var existingScreen = await _context.Screens.FindAsync(id);

            if (existingScreen == null)
                return null;

            existingScreen.TheatreId = screen.TheatreId;
            existingScreen.ScreenName = screen.ScreenName;
            existingScreen.TotalSeats = screen.TotalSeats;

            await _context.SaveChangesAsync();

            return existingScreen;
        }

        // Delete screen
        public async Task<bool> DeleteScreenAsync(int id)
        {
            var screen = await _context.Screens.FindAsync(id);

            if (screen == null)
                return false;

            _context.Screens.Remove(screen);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}