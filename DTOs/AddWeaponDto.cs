using System.ComponentModel.DataAnnotations;
using NetCoreAPI_Template_v2.Validations;

namespace NetCoreAPI_Template_v2.DTOs
{
    public class AddWeaponDto
    {
        [FirstLetterUpperCaseAttribute]
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, 100)]
        public int Damage { get; set; }
        [Required]
        public int CharacterId { get; set; }
    }
}