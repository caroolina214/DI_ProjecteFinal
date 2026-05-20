using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilAventures.Model.Models
{
    [Table("Guide")]
    public class Guide
    {
        [Key]
        public int id_guide { get; set; }

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

        [MaxLength(150)]
        public string specialty { get; set; }

        public string credentials { get; set; }

        [MaxLength(50)]
        public string experience_level { get; set; }

        public bool status { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
    }
}