using Microsoft.AspNetCore.Authorization;
using MovieTicketBookingSystem.Models;
using System.Security.Claims;

namespace MovieTicketBookingSystem.Authorization
{
    public class BookingOwnerHandler
        : AuthorizationHandler<BookingOwnerRequirement, Booking>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            BookingOwnerRequirement requirement,
            Booking booking)
        {
            // Allow Admin to access any booking
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Get the logged-in user's ID
            var userId = context.User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            // Check whether the logged-in user owns this booking
            if (int.TryParse(userId, out int loggedInUserId) &&
                booking.UserId == loggedInUserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}