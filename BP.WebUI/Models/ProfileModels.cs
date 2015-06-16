using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Entities.Users;

namespace BP.WebUI.Models
{
    public class FilterSkillViewModel
    {
        public UserSkillViewModel Skill { get; set; }
        public bool Include { get; set; }
    }

    public class CreateFilterViewModel
    {
        public int Id { get; set; }
        public List<FilterSkillViewModel> Skills { get; set; }
    }

    public class UserSkillViewModel
    {
        public BalSkill Skill { get; set; }
        public int Level { get; set; }
    }

    public class EditUserSkillsViewModel
    {
        public int Id { get; set; }
        public List<UserSkillViewModel> Skills { get; set; }
    }
}