using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;

        public ShowService(IShowRepository showRepository)
        {
            _showRepository = showRepository;
        }

        public async Task<IEnumerable<Show>> GetAllShowsAsync()
        {
            return await _showRepository.GetAllShowsAsync();
        }

        public async Task<Show?> GetShowByIdAsync(int id)
        {
            return await _showRepository.GetShowByIdAsync(id);
        }

        public async Task<Show> AddShowAsync(Show show)
        {
            return await _showRepository.AddShowAsync(show);
        }

        public async Task<Show?> UpdateShowAsync(int id, Show show)
        {
            return await _showRepository.UpdateShowAsync(id, show);
        }

        public async Task<bool> DeleteShowAsync(int id)
        {
            return await _showRepository.DeleteShowAsync(id);
        }

        public async Task<IEnumerable<ShowSeat>> GetShowSeatsAsync(int showId)
        {
            return await _showRepository.GetShowSeatsAsync(showId);
        }
    }
}