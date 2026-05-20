using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilAventures.Model.Models
{
    [Table("EquipmentStatus")]
    public class EquipmentStatus
    {
        [Key]
        public int id_status { get; set; }

        [Required]
        [MaxLength(50)]
        public string code { get; set; }

        public string description { get; set; }

        public virtual ICollection<Equipment> Equipments { get; set; }
    }
}