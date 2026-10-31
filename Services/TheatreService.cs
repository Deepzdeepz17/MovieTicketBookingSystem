using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class TheatreService : ITheatreService
    {
        private readonly ITheatreRepository _theatreRepository;

        public TheatreService(ITheatreRepository theatreRepository)
        {
            _theatreRepository = theatreRepository;
        }

        public async Task<IEnumerable<Theatre>> GetAllTheatresAsync()
        {
            return await _theatreRepository.GetAllTheatresAsync();
        }

        public async Task<Theatre?> GetTheatreByIdAsync(int id)
        {
            return await _theatreRepository.GetTheatreByIdAsync(id);
        }

        public async Task<Theatre> AddTheatreAsync(Theatre theatre)
        {
            return await _theatreRepository.AddTheatreAsync(theatre);
        }

        public async Task<Theatre?> UpdateTheatreAsync(int id, Theatre theatre)
        {
            return await _theatreRepository.UpdateTheatreAsync(id, theatre);
        }

        public async Task<bool> DeleteTheatreAsync(int id)
        {
            return await _theatreRepository.DeleteTheatreAsync(id);
        }
    }
}