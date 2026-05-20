using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilAventures.Model.Models
{
    [Table("BookingLine")]
    public class BookingLine
    {
        [Key]
        public int id_line { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public decimal price_at_moment { get; set; }
        [Required]
        public int bookingId { get; set; }
        [ForeignKey(nameof(bookingId))]
        public virtual Booking Booking { get; set; }

        public int? activityId { get; set; }
        [ForeignKey(nameof(activityId))]
        public virtual Activity Activity { get; set; }

        public int? equipmentId { get; set; }
        [ForeignKey(nameof(equipmentId))]
        public virtual Equipment Equipment { get; set; }
    }
}