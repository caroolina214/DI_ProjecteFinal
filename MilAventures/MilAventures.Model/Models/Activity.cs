using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilAventures.Model.Models
{
    [Table("Activity")]
    public class Activity
    {
        [Key]
        public int id_activity { get; set; }

        [Required]
        [MaxLength(150)]
        public string title { get; set; }

        public string description { get; set; }

        [Required]
        public DateTime init_date { get; set; }

        [Required]
        public DateTime end_date { get; set; }

        [Required]
        public int difficulty { get; set; }

        [Required]
        public int max_participants { get; set; }

        public string start_end_point { get; set; }

        [Required]
        public decimal price_per_person { get; set; }

        [Required]
        public int id_category { get; set; }

        [ForeignKey(nameof(id_category))]
        public virtual Category Category { get; set; }

        [Required]
        public int id_guide { get; set; }

        [ForeignKey(nameof(id_guide))]
        public virtual Guide Guide { get; set; }

        public virtual ICollection<BookingLine> BookingLines { get; set; }
    }
}