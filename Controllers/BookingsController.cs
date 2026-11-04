
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Services;
using System.Security.Claims;

namespace MovieTicketBookingSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        private int GetLoggedInUserId()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out int id))
            {
                throw new UnauthorizedAccessException(
                    "Invalid user identity.");
            }

            return id;
        }

        // GET: api/Bookings
        // Admin: Get all bookings
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings =
                await _bookingService.GetAllBookingsAsync();

            return Ok(bookings);
        }

        // GET: api/Bookings/my-bookings
        // Customer: Get only their own bookings
        [Authorize(Roles = "Customer")]
        [HttpGet("my-bookings")]
        public async Task<IActionResult> GetMyBookings()
        {
            int userId = GetLoggedInUserId();

            var bookings =
                await _bookingService.GetMyBookingsAsync(userId);

            return Ok(bookings);
        }

        // GET: api/Bookings/1
        // Admin: Any booking
        // Customer: Own booking only
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            int userId = GetLoggedInUserId();
            bool isAdmin = User.IsInRole("Admin");

            var booking =
                await _bookingService.GetBookingByIdAsync(
                    id, userId, isAdmin);

            if (booking == null)
                return NotFound("Booking not found.");

            return Ok(booking);
        }

        // POST: api/Bookings
        // Customer: Create booking
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> CreateBooking(
            CreateBookingRequest request)
        {
            try
            {
                int userId = GetLoggedInUserId();

                var booking =
                    await _bookingService.CreateBookingAsync(
                        request, userId);

                return CreatedAtAction(
                    nameof(GetBookingById),
                    new { id = booking.BookingId },
                    booking);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT: api/Bookings/1/cancel
        // Admin: Any booking
        // Customer: Own booking only
        [HttpPut("{id:int}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                int userId = GetLoggedInUserId();
                bool isAdmin = User.IsInRole("Admin");

                var booking =
                    await _bookingService.CancelBookingAsync(
                        id, userId, isAdmin);

                if (booking == null)
                    return NotFound("Booking not found.");

                return Ok(booking);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
