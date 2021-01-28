using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreAPI_Template_v2.Models
{
    [Table("Skill")]
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public List<CharacterSkill> CharacterSkill { get; set; }
    }
}