using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();

        Task<Booking?> GetBookingByIdAsync(int id);

        Task<Booking> CreateBookingAsync(CreateBookingRequest request);

        Task<Booking?> CancelBookingAsync(int id);
    }
}