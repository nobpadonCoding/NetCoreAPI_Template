using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v2.Models
{
    [Table("Weapon")]
    public class Weapon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Damage { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }

    }
}