using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilAventures.Model.Models
{
    [Table("Equipment")]
    public class Equipment
    {
        [Key]
        public int id_equipment { get; set; }

        [Required]
        [MaxLength(150)]
        public string title { get; set; }

        public string description { get; set; }

        [Required]
        public decimal price_per_day { get; set; }

        [Required]
        public int units { get; set; }

        [Required]
        public int min_stock { get; set; }

        [Required]
        public int id_category { get; set; }

        [ForeignKey(nameof(id_category))]
        public virtual Category Category { get; set; }

        [Required]
        public int id_status { get; set; }

        [ForeignKey(nameof(id_status))]
        public virtual EquipmentStatus EquipmentStatus { get; set; }

        public virtual ICollection<BookingLine> BookingLines { get; set; }
    }
}