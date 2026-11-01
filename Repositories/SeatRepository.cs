using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly ApplicationDbContext _context;

        public SeatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Seat>> GetAllSeatsAsync()
        {
            return await _context.Seats.ToListAsync();
        }

        public async Task<Seat?> GetSeatByIdAsync(int id)
        {
            return await _context.Seats.FindAsync(id);
        }

        public async Task<Seat> AddSeatAsync(Seat seat)
        {
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();

            return seat;
        }

        public async Task<Seat?> UpdateSeatAsync(int id, Seat seat)
        {
            var existingSeat = await _context.Seats.FindAsync(id);

            if (existingSeat == null)
                return null;

            existingSeat.ScreenId = seat.ScreenId;
            existingSeat.RowName = seat.RowName;
            existingSeat.SeatNumber = seat.SeatNumber;
            existingSeat.SeatType = seat.SeatType;

            await _context.SaveChangesAsync();

            return existingSeat;
        }

        public async Task<bool> DeleteSeatAsync(int id)
        {
            var seat = await _context.Seats.FindAsync(id);

            if (seat == null)
                return false;

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}