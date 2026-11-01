using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBookingSystem.Models
{
    public class BookingSeat
    {
        public int BookingSeatId { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        public int SeatId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(typeof(decimal), "0", "99999999")]
        public decimal Price { get; set; }

        public Booking? Booking { get; set; }

        public Seat? Seat { get; set; }
    }
}