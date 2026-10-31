
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();

        Task<Movie?> GetMovieByIdAsync(int id);

        Task<Movie> AddMovieAsync(Movie movie);

        Task<Movie?> UpdateMovieAsync(int id, Movie movie);

        Task<bool> DeleteMovieAsync(int id);
    }
}
