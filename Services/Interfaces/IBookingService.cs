
using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services
{
    public interface IBookingService
    {
        // Admin: Get all bookings
        Task<IEnumerable<Booking>> GetAllBookingsAsync();

        // Customer: Get own bookings
        Task<IEnumerable<Booking>> GetMyBookingsAsync(int userId);

        Task<Booking?> GetBookingByIdAsync(
            int id, int userId, bool isAdmin);

        Task<Booking> CreateBookingAsync(
            CreateBookingRequest request, int userId);

        Task<Booking?> CancelBookingAsync(
            int id, int userId, bool isAdmin);
    }
}
