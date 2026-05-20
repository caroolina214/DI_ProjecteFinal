using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilAventures.Model.Models
{
    [Table("BookingStatus")]
    public class BookingStatus
    {
        [Key]
        public int id_book_status { get; set; }

        [Required]
        [MaxLength(50)]
        public string code { get; set; }

        public string description { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}