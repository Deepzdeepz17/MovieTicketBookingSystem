using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;

        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<IEnumerable<Seat>> GetAllSeatsAsync()
        {
            return await _seatRepository.GetAllSeatsAsync();
        }

        public async Task<Seat?> GetSeatByIdAsync(int id)
        {
            return await _seatRepository.GetSeatByIdAsync(id);
        }

        public async Task<Seat> AddSeatAsync(Seat seat)
        {
            return await _seatRepository.AddSeatAsync(seat);
        }

        public async Task<Seat?> UpdateSeatAsync(int id, Seat seat)
        {
            return await _seatRepository.UpdateSeatAsync(id, seat);
        }

        public async Task<bool> DeleteSeatAsync(int id)
        {
            return await _seatRepository.DeleteSeatAsync(id);
        }
    }
}