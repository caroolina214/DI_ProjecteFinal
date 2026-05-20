using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilAventures.Model.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int id_category { get; set; }

        [Required]
        [MaxLength(50)]
        public string code { get; set; }

        public string description { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Equipment> Equipments { get; set; }
    }
}