using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces
{
    public interface IShowRepository
    {
        Task<IEnumerable<Show>> GetAllShowsAsync();
        Task<Show?> GetShowByIdAsync(int id);
        Task<Show> AddShowAsync(Show show);
        Task<Show?> UpdateShowAsync(int id, Show show);
        Task<bool> DeleteShowAsync(int id);
        Task<IEnumerable<ShowSeat>> GetShowSeatsAsync(int showId);
    }
}