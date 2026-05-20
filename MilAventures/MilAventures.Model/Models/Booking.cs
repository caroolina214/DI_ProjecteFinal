using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilAventures.Model.Models
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        public int id_booking { get; set; }

        [Required]
        public DateTime created_at { get; set; }

        [Required]
        public decimal total_price { get; set; }

        [Required]
        public int participants { get; set; }

        public string notes { get; set; }

        [Required]
        public int id_client { get; set; }

        [ForeignKey(nameof(id_client))]
        public virtual Client Client { get; set; }

        [Required]
        public int id_book_status { get; set; }

        [ForeignKey(nameof(id_book_status))]
        public virtual BookingStatus BookingStatus { get; set; }

        public virtual ICollection<BookingLine> BookingLines { get; set; }
    }
}