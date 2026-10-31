
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<Movie> AddMovieAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task<Movie?> UpdateMovieAsync(int id, Movie movie)
        {
            var existingMovie = await _context.Movies.FindAsync(id);

            if (existingMovie == null)
                return null;

            existingMovie.Title = movie.Title;
            existingMovie.Description = movie.Description;
            existingMovie.DurationMinutes = movie.DurationMinutes;
            existingMovie.ReleaseDate = movie.ReleaseDate;
            existingMovie.Language = movie.Language;
            existingMovie.Genre = movie.Genre;

            await _context.SaveChangesAsync();

            return existingMovie;
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
