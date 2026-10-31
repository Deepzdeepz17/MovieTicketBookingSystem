using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IScreenRepository _screenRepository;

        public ScreenService(IScreenRepository screenRepository)
        {
            _screenRepository = screenRepository;
        }

        public async Task<IEnumerable<Screen>> GetAllScreensAsync()
        {
            return await _screenRepository.GetAllScreensAsync();
        }

        public async Task<Screen?> GetScreenByIdAsync(int id)
        {
            return await _screenRepository.GetScreenByIdAsync(id);
        }

        public async Task<Screen> AddScreenAsync(Screen screen)
        {
            return await _screenRepository.AddScreenAsync(screen);
        }

        public async Task<Screen?> UpdateScreenAsync(int id, Screen screen)
        {
            return await _screenRepository.UpdateScreenAsync(id, screen);
        }

        public async Task<bool> DeleteScreenAsync(int id)
        {
            return await _screenRepository.DeleteScreenAsync(id);
        }
    }
}