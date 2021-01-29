using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPI_Template_v2.DTOs;
using NetCoreAPI_Template_v2.DTOs.Fight;
using NetCoreAPI_Template_v2.Services.Charecter;

namespace NetCoreAPI_Template_v2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly IcaracterService _charService;

        public CharacterController(IcaracterService charService)
        {
            _charService = charService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCharacters()
        {
            return Ok(await _charService.GetAllCharacters());
        }

        [HttpGet("{characterId}")]
        public async Task<IActionResult> GetCharacterById(int characterId)
        {
            return Ok(await _charService.GetCharacterById(characterId));
        }

        [HttpPost("addweapon")]
        public async Task<IActionResult> AddWeapon(AddWeaponDto newWeapon)
        {
            return Ok(await _charService.AddWeapon(newWeapon));
        }

        [HttpPost("addcharacterskill")]
        public async Task<IActionResult> AddCharacterSkill(AddCharacterSkillDto newCharSkill)
        {
            return Ok(await _charService.AddCharacterSkill(newCharSkill));
        }

        [HttpPost("addskill")]
        public async Task<IActionResult> AddSkill(AddskillDto newskillDto)
        {
            return Ok(await _charService.AddSkill(newskillDto));
        }

        // [HttpGet]
        // public async Task<IActionResult> GetAllSkills()
        // {
        //     return Ok(await _charService.GetAllSkills());
        // }

        [HttpPut("weaponatk")]
        public async Task<IActionResult> WeaponAtk(WeaponAtkDto request)
        {
            return Ok(await _charService.WeaponAtk(request));
        }

        [HttpPut("skillatk")]
        public async Task<IActionResult> SkillAtk(SkillAtkDto request)
        {
            return Ok(await _charService.SkillAtk(request));
        }

        [HttpDelete("removeweapon")]
        public async Task<IActionResult> RemoveWeapon(int characterId)
        {
            return Ok(await _charService.RemoveWeapon(characterId));
        }

        [HttpDelete("removeskill")]
        public async Task<IActionResult> RemoveSkill(int characterId)
        {
            return Ok(await _charService.RemoveSkill(characterId));
        }
    }
}