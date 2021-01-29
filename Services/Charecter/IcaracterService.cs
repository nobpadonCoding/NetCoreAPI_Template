using System.Collections.Generic;
using System.Threading.Tasks;
using NetCoreAPI_Template_v2.DTOs;
using NetCoreAPI_Template_v2.DTOs.Fight;
using NetCoreAPI_Template_v2.Models;

namespace NetCoreAPI_Template_v2.Services.Charecter
{
    public interface IcaracterService
    {
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();
        Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int characterId);
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
        Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newSkill);
        Task<ServiceResponse<List<GetSkillDto>>> GetAllSkills();
        Task<ServiceResponse<GetSkillDto>> AddSkill(AddskillDto newSkillDto);
        Task<ServiceResponse<AttackResultDto>> WeaponAtk(WeaponAtkDto request);
        Task<ServiceResponse<AttackResultDto>> SkillAtk(SkillAtkDto request);
        Task<ServiceResponse<GetCharacterDto>> RemoveWeapon(int characterId);
        Task<ServiceResponse<GetCharacterDto>> RemoveSkill(int characterId);
    }
}