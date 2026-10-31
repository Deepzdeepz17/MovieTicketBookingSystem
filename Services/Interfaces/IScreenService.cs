using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services
{
    public interface IScreenService
    {
        Task<IEnumerable<Screen>> GetAllScreensAsync();

        Task<Screen?> GetScreenByIdAsync(int id);

        Task<Screen> AddScreenAsync(Screen screen);

        Task<Screen?> UpdateScreenAsync(int id, Screen screen);

        Task<bool> DeleteScreenAsync(int id);
    }
}