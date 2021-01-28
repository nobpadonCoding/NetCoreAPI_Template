using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreAPI_Template_v2.Data;
using NetCoreAPI_Template_v2.DTOs;
using NetCoreAPI_Template_v2.Models;

namespace NetCoreAPI_Template_v2.Services.Charecter
{
    public class CharacterService : IcaracterService
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _log;

        public CharacterService(AppDBContext dbContext, IMapper mapper, ILogger<CharacterService> log)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _log = log;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newSkill)
        {
            _log.LogInformation("Start Add CharacterSkill process.");
            _log.LogInformation($"CharacterId : {newSkill.CharacterId}, SkillId : {newSkill.SkillId}");
            var character = await _dbContext.Characters.Include(x=>x.CharacterSkill).ThenInclude(x=>x.Skill).FirstOrDefaultAsync(x => x.Id == newSkill.CharacterId);
            if (character == null)
            {
                _log.LogError("Character error not found.");
                return ResponseResult.Failure<GetCharacterDto>("Character not found.");
            }

            _log.LogInformation("Character founded.");

            var skill = await _dbContext.Skills.FirstOrDefaultAsync(x=>x.Id == newSkill.SkillId);
            if (skill == null)
            {
                _log.LogError("Skill not found.");
                return ResponseResult.Failure<GetCharacterDto>("Skill not found.");
            }

            _log.LogInformation("Skill founded.");

            var characterSkill = new CharacterSkill
            {
                Character = character,
                Skill = skill
            };

            _dbContext.CharacterSkills.Add(characterSkill);
            await _dbContext.SaveChangesAsync();
            _log.LogInformation("Success.");

            var dto = _mapper.Map<GetCharacterDto>(character);
            _log.LogInformation("End.");

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var character = await _dbContext.Characters.FirstOrDefaultAsync(x=>x.Id == newWeapon.CharacterId);
            if (character == null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Character not found.");
            }

             var weapon = new Weapon
            {
                Name = newWeapon.Name,
                Damage = newWeapon.Damage,
                Character = character
            };

            _dbContext.Weapons.Add(weapon);
            await _dbContext.SaveChangesAsync();

            var dto = _mapper.Map<GetCharacterDto>(character);
            return ResponseResult.Success(dto);
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var characters = await _dbContext.Characters.Include(x=>x.Weapon).AsNoTracking().ToListAsync();
            var dto = _mapper.Map<List<GetCharacterDto>>(characters);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int characterId)
        {
            var character = await _dbContext.Characters.Include(x=>x.Weapon).FirstOrDefaultAsync(x => x.Id == characterId);
            if (character == null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Character not found.");
            }

            var dto = _mapper.Map<GetCharacterDto>(character);

            return ResponseResult.Success(dto);
        }
    }
}