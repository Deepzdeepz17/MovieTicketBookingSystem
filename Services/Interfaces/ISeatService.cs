using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services
{
    public interface ISeatService
    {
        Task<IEnumerable<Seat>> GetAllSeatsAsync();
        Task<Seat?> GetSeatByIdAsync(int id);
        Task<Seat> AddSeatAsync(Seat seat);
        Task<Seat?> UpdateSeatAsync(int id, Seat seat);
        Task<bool> DeleteSeatAsync(int id);
    }
}