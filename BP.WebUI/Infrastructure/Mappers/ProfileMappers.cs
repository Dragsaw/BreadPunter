﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BP.WebUI.Models;
using BP.BLL.Interface.Entities;

namespace BP.WebUI.Infrastructure.Mappers
{
    public static class ProfileMappers
    {
        public static FilterSkillViewModel ToMvc(this BllSkill skill, int level = 0, bool include = false)
        {
            return new FilterSkillViewModel
            {
                Skill = new UserSkillViewModel { Skill = skill, Level = level },
                Include = include
            };
        }

        public static BllFilter ToBal(this FilterViewModel filter)
        {
            return new BllFilter
            {
                Id = filter.Id,
                LastViewed = filter.LastViewed,
                Skills = filter.Skills.Where(x => x.Include).ToDictionary(k => k.Skill.Skill, v => v.Skill.Level)
            };
        }
    }
}