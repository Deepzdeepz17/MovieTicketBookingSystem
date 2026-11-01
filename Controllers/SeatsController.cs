using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services;

namespace MovieTicketBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSeats()
        {
            var seats = await _seatService.GetAllSeatsAsync();
            return Ok(seats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatById(int id)
        {
            var seat = await _seatService.GetSeatByIdAsync(id);

            if (seat == null)
                return NotFound("Seat not found.");

            return Ok(seat);
        }

        [HttpPost]
        public async Task<IActionResult> AddSeat(Seat seat)
        {
            var createdSeat = await _seatService.AddSeatAsync(seat);

            return CreatedAtAction(
                nameof(GetSeatById),
                new { id = createdSeat.SeatId },
                createdSeat);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(int id, Seat seat)
        {
            var updatedSeat = await _seatService.UpdateSeatAsync(id, seat);

            if (updatedSeat == null)
                return NotFound("Seat not found.");

            return Ok(updatedSeat);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(int id)
        {
            var deleted = await _seatService.DeleteSeatAsync(id);

            if (!deleted)
                return NotFound("Seat not found.");

            return Ok("Seat deleted successfully.");
        }
    }
}