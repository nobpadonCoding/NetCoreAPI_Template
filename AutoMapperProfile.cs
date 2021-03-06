﻿using AutoMapper;
using NetCoreAPI_Template_v2.DTOs;
using NetCoreAPI_Template_v2.DTOs.Company;
using NetCoreAPI_Template_v2.DTOs.Product;
using NetCoreAPI_Template_v2.Models;
using NetCoreAPI_Template_v2.Models.Company;
using NetCoreAPI_Template_v2.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreAPI_Template_v2
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>().ForMember(dto => dto.Skills, x => x.MapFrom(x => x.CharacterSkill.Select(cs => cs.Skill)));
            CreateMap<Weapon,GetweponDto>();
            CreateMap<Skill, GetSkillDto>();
            CreateMap<Product, GetProductDto>();
            CreateMap<ProductGroup, GetProductGroupDto>();
            CreateMap<Employee, GetEmployeeDto>();
            CreateMap<Position, GetPositionDto>();
            CreateMap<Department,GetDepartmentDto>();
        }
    }
}