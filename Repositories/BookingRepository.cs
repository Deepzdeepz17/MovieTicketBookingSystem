
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin: Get all bookings
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Show)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        // Customer: Get only their own bookings
        public async Task<IEnumerable<Booking>> GetMyBookingsAsync(int userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Show)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        // Admin: Get any booking
        // Customer: Get own booking only
        public async Task<Booking?> GetBookingByIdAsync(
            int id, int userId, bool isAdmin)
        {
            var query = _context.Bookings
                .Include(b => b.Show)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(b => b.UserId == userId);
            }

            return await query.FirstOrDefaultAsync(
                b => b.BookingId == id);
        }

        // Customer: Create booking
        public async Task<Booking> CreateBookingAsync(
            CreateBookingRequest request, int userId)
        {
            if (request.SeatIds == null ||
                request.SeatIds.Count == 0 ||
                request.SeatIds.Distinct().Count() != request.SeatIds.Count)
            {
                throw new InvalidOperationException(
                    "Select at least one seat and do not repeat seat IDs.");
            }

            // Get logged-in customer
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            if (user.Role != "Customer")
                throw new InvalidOperationException(
                    "Only customers can create bookings.");

            if (string.IsNullOrWhiteSpace(request.CustomerPhone) ||
                request.CustomerPhone.Length > 15)
            {
                throw new InvalidOperationException(
                    "Enter a valid customer phone number.");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Check whether show exists
                var show = await _context.Shows
                    .FirstOrDefaultAsync(s => s.ShowId == request.ShowId);

                if (show == null)
                    throw new InvalidOperationException("Show not found.");

                // Check whether selected seats belong to this show
                var matchingSeatCount = await _context.ShowSeats
                    .CountAsync(ss =>
                        ss.ShowId == request.ShowId &&
                        request.SeatIds.Contains(ss.SeatId));

                if (matchingSeatCount != request.SeatIds.Count)
                {
                    throw new InvalidOperationException(
                        "One or more selected seats do not belong to this show.");
                }

                // Atomically reserve available seats
                var updatedSeatCount = await _context.ShowSeats
                    .Where(ss =>
                        ss.ShowId == request.ShowId &&
                        request.SeatIds.Contains(ss.SeatId) &&
                        ss.Status == "Available")
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(
                            ss => ss.Status,
                            "Booked"));

                if (updatedSeatCount != request.SeatIds.Count)
                {
                    throw new InvalidOperationException(
                        "One or more selected seats are already booked. Please select different seats.");
                }

                // Create booking
                var booking = new Booking
                {
                    UserId = user.UserId,
                    ShowId = request.ShowId,

                    CustomerName = user.Name,
                    CustomerEmail = user.Email,
                    CustomerPhone = request.CustomerPhone,

                    BookingReference =
                        Guid.NewGuid().ToString("N")[..12].ToUpper(),

                    BookingDate = DateTime.UtcNow,

                    TotalAmount =
                        show.TicketPrice * request.SeatIds.Count,

                    Status = "Confirmed"
                };

                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();

                // Create booking-seat records
                var bookingSeats = request.SeatIds.Select(seatId =>
                    new BookingSeat
                    {
                        BookingId = booking.BookingId,
                        SeatId = seatId,
                        Price = show.TicketPrice
                    }).ToList();

                await _context.BookingSeats.AddRangeAsync(bookingSeats);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return booking;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Admin: Cancel any booking
        // Customer: Cancel own booking only
        public async Task<Booking?> CancelBookingAsync(
            int id, int userId, bool isAdmin)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            var query = _context.Bookings
                .Include(b => b.BookingSeats)
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(b => b.UserId == userId);
            }

            var booking = await query.FirstOrDefaultAsync(
                b => b.BookingId == id);

            if (booking == null)
                return null;

            if (booking.Status != "Confirmed")
                throw new InvalidOperationException(
                    "Only confirmed bookings can be cancelled.");

            // Get booked seat IDs
            var seatIds = booking.BookingSeats
                .Select(bs => bs.SeatId)
                .ToList();

            // Make seats available again
            var showSeats = await _context.ShowSeats
                .Where(ss =>
                    ss.ShowId == booking.ShowId &&
                    seatIds.Contains(ss.SeatId) &&
                    ss.Status == "Booked")
                .ToListAsync();

            foreach (var showSeat in showSeats)
            {
                showSeat.Status = "Available";
            }

            booking.Status = "Cancelled";

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return booking;
        }
    }
}
