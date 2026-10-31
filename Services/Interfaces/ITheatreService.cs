using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services
{
    public interface ITheatreService
    {
        Task<IEnumerable<Theatre>> GetAllTheatresAsync();

        Task<Theatre?> GetTheatreByIdAsync(int id);

        Task<Theatre> AddTheatreAsync(Theatre theatre);

        Task<Theatre?> UpdateTheatreAsync(int id, Theatre theatre);

        Task<bool> DeleteTheatreAsync(int id);
    }
}