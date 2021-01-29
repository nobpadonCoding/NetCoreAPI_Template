using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetCoreAPI_Template_v2.Data;
using NetCoreAPI_Template_v2.DTOs;
using NetCoreAPI_Template_v2.DTOs.Fight;
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

        public async Task<ServiceResponse<List<GetSkillDto>>> GetAllSkills()
        {
            var skills = await _dbContext.Skills.ToListAsync();
            var dto = _mapper.Map<List<GetSkillDto>>(skills);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetSkillDto>> AddSkill(AddskillDto newskillDto)
        {
            var skills = await _dbContext.Skills.FirstOrDefaultAsync(x=>x.Name == newskillDto.Name);
            if (skills != null)
            {
                return ResponseResult.Failure<GetSkillDto>("skills duplicate.");
            }

             var skill = new Skill
            {
                Name = newskillDto.Name,
                Damage = newskillDto.Damage,
            };

            _dbContext.Skills.Add(skill);
            await _dbContext.SaveChangesAsync();

            var dto = _mapper.Map<GetSkillDto>(skill);
            return ResponseResult.Success(dto);
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
            var characters = await _dbContext.Characters
                .Include(x=>x.Weapon).Include(x => x.CharacterSkill)
                .ThenInclude(x => x.Skill)
                .AsNoTracking()
                .ToListAsync();
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

        public async Task<ServiceResponse<AttackResultDto>> WeaponAtk(WeaponAtkDto request)
        {
            try
            {
                var attacker = await _dbContext.Characters
                .Include(x => x.Weapon)
                .FirstOrDefaultAsync(x => x.Id == request.AttackerId);

                if (attacker is null)
                {
                    var msg = $"This attackerId {request.AttackerId} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttackResultDto>(msg);
                }

                var opponent = await _dbContext.Characters
                .Include(x => x.Weapon)
                .FirstOrDefaultAsync(x => x.Id == request.OpponentId);

                if (opponent is null)
                {
                    var msg = $"This opponentId {request.OpponentId} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttackResultDto>(msg);
                }

                int damage;
                damage = attacker.Weapon.Damage + attacker.Strength;
                damage -= opponent.Defense;

                if (damage > 0)
                {
                    opponent.HisPoints -= damage;
                }

                string atkResultMessage;

                if (opponent.HisPoints <= 0)
                {
                    atkResultMessage = $"{opponent.Name} is dead.";
                }
                else
                {
                    atkResultMessage = $"{opponent.Name} HP Remain {opponent.HisPoints}";
                }

                _dbContext.Characters.Update(opponent);
                await _dbContext.SaveChangesAsync();

                var dto = new AttackResultDto
                {
                    AttackerName = attacker.Name,
                    AttackHP = attacker.HisPoints,
                    OpponentName = opponent.Name,
                    OpponentHP = opponent.HisPoints,
                    Damage = damage,
                    AttackResultMessage = atkResultMessage
                };

                _log.LogInformation("Weapon attack done.");
                return ResponseResult.Success(dto);

            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return ResponseResult.Failure<AttackResultDto>(ex.Message);
            }
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAtk(SkillAtkDto request)
        {
            try
            {
                var attacker = await _dbContext.Characters
                .Include(x => x.CharacterSkill).ThenInclude(x => x.Skill)
                .FirstOrDefaultAsync(x => x.Id == request.AttackerId);

                if (attacker is null)
                {
                    var msg = $"This attackerId {request.AttackerId} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttackResultDto>(msg);
                }

                var opponent = await _dbContext.Characters
                .Include(x => x.Weapon)
                .FirstOrDefaultAsync(x => x.Id == request.OpponentId);

                if (opponent is null)
                {
                    var msg = $"This opponentId {request.OpponentId} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttackResultDto>(msg);
                }

                var charSkill = await _dbContext.CharacterSkills.Include(x => x.Skill)
                .FirstOrDefaultAsync(x => x.CharacterId == request.AttackerId && x.SkillId == request.SkillId);
                if (charSkill is null)
                {
                    var msg = $"ไม่พบ skill {request.OpponentId}.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<AttackResultDto>(msg);
                }

                int damage;
                damage = charSkill.Skill.Damage + attacker.Intelligence;
                damage -= opponent.Defense;

                if (damage > 0)
                {
                    opponent.HisPoints -= damage;
                    _log.LogInformation($"Skill Dmg = {charSkill.Skill.Damage}");
                    _log.LogInformation($"{attacker.Name} Intelligent = {attacker.Intelligence}");
                    _log.LogInformation($"{opponent.Name} Defend = {opponent.Defense}");
                    _log.LogInformation($"Skill Dmg = {charSkill.Skill.Damage} Total damage = {damage}");
                }

                string atkResultMessage;

                if (opponent.HisPoints <= 0)
                {
                    atkResultMessage = $"{opponent.Name} is dead.";
                }
                else
                {
                    atkResultMessage = $"{opponent.Name} HP Remain {opponent.HisPoints}";
                }

                _dbContext.Characters.Update(opponent);
                await _dbContext.SaveChangesAsync();

                var dto = new AttackResultDto
                {
                    AttackerName = attacker.Name,
                    AttackHP = attacker.HisPoints,
                    OpponentName = opponent.Name,
                    OpponentHP = opponent.HisPoints,
                    Damage = damage,
                    AttackResultMessage = atkResultMessage
                };

                _log.LogInformation(atkResultMessage);
                _log.LogInformation("Skill attack done.");

                return ResponseResult.Success(dto);

            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return ResponseResult.Failure<AttackResultDto>(ex.Message);
            }
        }

        public async Task<ServiceResponse<GetCharacterDto>> RemoveWeapon(int characterId)
        {
            var character = await _dbContext.Characters.Include(x=>x.Weapon).FirstOrDefaultAsync(x => x.Id == characterId);
            if (character is null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Character not found.");
            }

            var charSkill = await _dbContext.CharacterSkills.Where(x => x.CharacterId == characterId).ToListAsync();
            if (charSkill is null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Weapon not found.");
            }

            _dbContext.CharacterSkills.RemoveRange(charSkill); // remove มากกว่าหนึ่ง(list)ใช้ RemoveRange()
            await _dbContext.SaveChangesAsync();

            var dto = _mapper.Map<GetCharacterDto>(character);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetCharacterDto>> RemoveSkill(int characterId)
        {
            var character = await _dbContext.Characters.Include(x=>x.Weapon).FirstOrDefaultAsync(x => x.Id == characterId);
            if (character is null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Character not found.");
            }

            var weapon = await _dbContext.Weapons.Where(x => x.CharacterId == characterId).FirstOrDefaultAsync();
            if (weapon is null)
            {
                return ResponseResult.Failure<GetCharacterDto>("Weapon not found.");
            }
            
            _dbContext.Weapons.Remove(weapon);
            await _dbContext.SaveChangesAsync();

            var dto = _mapper.Map<GetCharacterDto>(character);

            return ResponseResult.Success(dto);
        }
    }
}