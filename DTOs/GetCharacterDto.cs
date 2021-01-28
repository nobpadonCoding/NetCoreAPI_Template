using System.Collections.Generic;

namespace NetCoreAPI_Template_v2.DTOs
{
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HitPoints { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Intelligence { get; set; }
        public GetweponDto Weapon { get; set; }
        public List<GetSkillDto> Skills { get; set; }
    }
}