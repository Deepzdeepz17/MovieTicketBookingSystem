using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories
{
    public class TheatreRepository : ITheatreRepository
    {
        private readonly ApplicationDbContext _context;

        public TheatreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all theatres
        public async Task<IEnumerable<Theatre>> GetAllTheatresAsync()
        {
            return await _context.Theatres.ToListAsync();
        }

        // Get theatre by ID
        public async Task<Theatre?> GetTheatreByIdAsync(int id)
        {
            return await _context.Theatres.FindAsync(id);
        }

        // Add a new theatre
        public async Task<Theatre> AddTheatreAsync(Theatre theatre)
        {
            await _context.Theatres.AddAsync(theatre);
            await _context.SaveChangesAsync();

            return theatre;
        }

        // Update theatre
        public async Task<Theatre?> UpdateTheatreAsync(int id, Theatre theatre)
        {
            var existingTheatre = await _context.Theatres.FindAsync(id);

            if (existingTheatre == null)
                return null;

            existingTheatre.Name = theatre.Name;
            existingTheatre.Location = theatre.Location;

            await _context.SaveChangesAsync();

            return existingTheatre;
        }

        // Delete theatre
        public async Task<bool> DeleteTheatreAsync(int id)
        {
            var theatre = await _context.Theatres.FindAsync(id);

            if (theatre == null)
                return false;

            _context.Theatres.Remove(theatre);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}