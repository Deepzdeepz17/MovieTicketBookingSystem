using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBookingSystem.Models
{
    [Index(nameof(ScreenId), nameof(RowName), nameof(SeatNumber), IsUnique = true)]
    public class Seat
    {
        public int SeatId { get; set; }

        [Required]
        public int ScreenId { get; set; }

        [Required]
        [StringLength(5)]
        public string RowName { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int SeatNumber { get; set; }

        [Required]
        [StringLength(30)]
        public string SeatType { get; set; } = string.Empty;

        public Screen? Screen { get; set; }
    }
}