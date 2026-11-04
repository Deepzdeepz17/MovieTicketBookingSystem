
using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        // Admin: Get all bookings
        Task<IEnumerable<Booking>> GetAllBookingsAsync();

        // Customer: Get only their own bookings
        Task<IEnumerable<Booking>> GetMyBookingsAsync(int userId);

        // Admin: Get any booking
        // Customer: Get own booking only
        Task<Booking?> GetBookingByIdAsync(
            int id, int userId, bool isAdmin);

        // Customer: Create booking
        Task<Booking> CreateBookingAsync(
            CreateBookingRequest request, int userId);

        // Admin: Cancel any booking
        // Customer: Cancel own booking only
        Task<Booking?> CancelBookingAsync(
            int id, int userId, bool isAdmin);
    }
}
