
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Services;
using MovieTicketBookingSystem.Authorization;
using System.Security.Claims;

namespace MovieTicketBookingSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IAuthorizationService _authorizationService;

        public BookingsController(
            IBookingService bookingService,
            IAuthorizationService authorizationService)
        {
            _bookingService = bookingService;
            _authorizationService = authorizationService;
        }

        // Get logged-in user's ID
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
        [Authorize(Roles = "Admin,Customer")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            // Retrieve booking first
            var booking =
                await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
                return NotFound("Booking not found.");

            // Check custom authorization
            var authorizationResult =
                await _authorizationService.AuthorizeAsync(
                    User,
                    booking,
                    new BookingOwnerRequirement());

            if (!authorizationResult.Succeeded)
                return Forbid();

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
        [Authorize(Roles = "Admin,Customer")]
        [HttpPut("{id:int}/cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                // Retrieve booking first
                var booking =
                    await _bookingService.GetBookingByIdAsync(id);

                if (booking == null)
                    return NotFound("Booking not found.");

                // Check custom authorization
                var authorizationResult =
                    await _authorizationService.AuthorizeAsync(
                        User,
                        booking,
                        new BookingOwnerRequirement());

                if (!authorizationResult.Succeeded)
                    return Forbid();

                // Cancel only after authorization succeeds
                var cancelledBooking =
                    await _bookingService.CancelBookingAsync(id);

                if (cancelledBooking == null)
                    return NotFound("Booking not found.");

                return Ok(cancelledBooking);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
