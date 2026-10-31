
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();

        Task<Movie?> GetMovieByIdAsync(int id);

        Task<Movie> AddMovieAsync(Movie movie);

        Task<Movie?> UpdateMovieAsync(int id, Movie movie);

        Task<bool> DeleteMovieAsync(int id);
    }
}
