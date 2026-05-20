using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilAventures.Model.Models
{
    [Table("Client")]
    public class Client
    {
        [Key]
        public int id_client { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        [Required]
        [MaxLength(100)]
        public string surname { get; set; }

        [MaxLength(150)]
        public string email { get; set; }

        [MaxLength(30)]
        public string phone { get; set; }

        public string photo { get; set; }

        public bool status { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}