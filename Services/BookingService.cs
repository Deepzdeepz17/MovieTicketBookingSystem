using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        // Admin: Get all bookings
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllBookingsAsync();
        }

        // Customer: Get only their own bookings
        public async Task<IEnumerable<Booking>> GetMyBookingsAsync(int userId)
        {
            return await _bookingRepository.GetMyBookingsAsync(userId);
        }

        // Get booking by ID
        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetBookingByIdAsync(id);
        }

        // Customer: Create booking
        public async Task<Booking> CreateBookingAsync(
            CreateBookingRequest request, int userId)
        {
            return await _bookingRepository.CreateBookingAsync(
                request, userId);
        }

        // Cancel booking
        public async Task<Booking?> CancelBookingAsync(int id)
        {
            return await _bookingRepository.CancelBookingAsync(id);
        }
    }
}