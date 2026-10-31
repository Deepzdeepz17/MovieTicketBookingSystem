using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingSystem.Models
{
    public class Screen
    {
        public int ScreenId { get; set; }

        [Required]
        public int TheatreId { get; set; }

        [Required]
        [StringLength(100)]
        public string ScreenName { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int TotalSeats { get; set; }

        public Theatre? Theatre { get; set; }
    }
}