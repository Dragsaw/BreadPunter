using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BP.WebUI.Models;
using BP.BLL.Interface.Entities;

namespace BP.WebUI.Infrastructure.Mappers
{
    public static class ProfileMappers
    {
        public static FilterSkillViewModel ToMvc(this BalSkill skill)
        {
            return new FilterSkillViewModel
            {
                Skill = new UserSkillViewModel { Skill = skill }
            };
        }

        public static BalFilter ToBal(this CreateFilterViewModel filter)
        {
            return new BalFilter
            {
                LastViewed = null,
                Skills = filter.Skills.Where(x => x.Include).ToDictionary(k => k.Skill.Skill, v => v.Skill.Level)
            };
        }
    }
}