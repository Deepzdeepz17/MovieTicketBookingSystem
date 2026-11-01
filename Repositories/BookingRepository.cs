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

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Show)
                .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Show)
                .Include(b => b.BookingSeats)
                .ThenInclude(bs => bs.Seat)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task<Booking> CreateBookingAsync(CreateBookingRequest request)
        {
            if (request.SeatIds == null ||
                request.SeatIds.Count == 0 ||
                request.SeatIds.Distinct().Count() != request.SeatIds.Count)
            {
                throw new InvalidOperationException(
                    "Select at least one seat and do not repeat seat IDs.");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            var show = await _context.Shows
                .FirstOrDefaultAsync(s => s.ShowId == request.ShowId);

            if (show == null)
                throw new InvalidOperationException("Show not found.");

            var showSeats = await _context.ShowSeats
                .Where(ss => ss.ShowId == request.ShowId &&
                             request.SeatIds.Contains(ss.SeatId))
                .ToListAsync();

            if (showSeats.Count != request.SeatIds.Count)
                throw new InvalidOperationException(
                    "One or more selected seats do not belong to this show.");

            if (showSeats.Any(ss => ss.Status != "Available"))
                throw new InvalidOperationException(
                    "One or more selected seats are already booked.");

            var booking = new Booking
            {
                BookingReference = Guid.NewGuid().ToString("N")[..12].ToUpper(),
                ShowId = request.ShowId,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone,
                BookingDate = DateTime.UtcNow,
                TotalAmount = show.TicketPrice * request.SeatIds.Count,
                Status = "Confirmed"
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            foreach (var showSeat in showSeats)
            {
                var bookingSeat = new BookingSeat
                {
                    BookingId = booking.BookingId,
                    SeatId = showSeat.SeatId,
                    Price = show.TicketPrice
                };

                await _context.BookingSeats.AddAsync(bookingSeat);

                showSeat.Status = "Booked";
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return booking;
        }

        public async Task<Booking?> CancelBookingAsync(int id)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            var booking = await _context.Bookings
                .Include(b => b.BookingSeats)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                return null;

            if (booking.Status != "Confirmed")
                throw new InvalidOperationException(
                    "Only confirmed bookings can be cancelled.");

            var seatIds = booking.BookingSeats
                .Select(bs => bs.SeatId)
                .ToList();

            var showSeats = await _context.ShowSeats
                .Where(ss => ss.ShowId == booking.ShowId &&
                             seatIds.Contains(ss.SeatId))
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