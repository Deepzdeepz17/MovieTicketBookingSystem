
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces
{
    public interface ITheatreRepository
    {
        Task<IEnumerable<Theatre>> GetAllTheatresAsync();

        Task<Theatre?> GetTheatreByIdAsync(int id);

        Task<Theatre> AddTheatreAsync(Theatre theatre);

        Task<Theatre?> UpdateTheatreAsync(int id, Theatre theatre);

        Task<bool> DeleteTheatreAsync(int id);
    }
}
