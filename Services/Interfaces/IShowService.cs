using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services
{
    public interface IShowService
    {
        Task<IEnumerable<Show>> GetAllShowsAsync();
        Task<Show?> GetShowByIdAsync(int id);
        Task<Show> AddShowAsync(Show show);
        Task<Show?> UpdateShowAsync(int id, Show show);
        Task<bool> DeleteShowAsync(int id);
    }
}