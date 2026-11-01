
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingSystem.DTOs.Bookings
{
    public class CreateBookingRequest
    {
        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required]
        public int ShowId { get; set; }

        [Required]
        [MinLength(1)]
        public List<int> SeatIds { get; set; } = new List<int>();
    }
}
