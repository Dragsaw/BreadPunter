using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace BP.WebUI.Models
{
    public class FilterSkillViewModel
    {
        public UserSkillViewModel Skill { get; set; }
        public bool Include { get; set; }
    }

    public class FilterViewModel
    {
        public int Id { get; set; }
        public List<FilterSkillViewModel> Skills { get; set; }
        public DateTime? LastViewed { get; set; }
    }

    public class UserSkillViewModel
    {
        public BalSkill Skill { get; set; }
        [Range(0, 5)]
        public int Level { get; set; }
    }

    public class EditUserSkillsViewModel
    {
        public int Id { get; set; }
        public List<UserSkillViewModel> Skills { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public string ImageType { get; set; }
        public string About { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string BirthDate { get; set; }

        public void GetInfo(BalProgrammer user)
        {
            Name = user.Name;
            About = user.About;
            ImageType = user.ImageType;
            if (user.BirthDate != null)
                BirthDate = ((DateTime)user.BirthDate).ToShortDateString();
        }

        public void SetUserInfo(BalProgrammer user)
        {
            user.Name = Name;
            user.About = About;
            if (BirthDate != null)
                user.BirthDate = DateTime.Parse(BirthDate);
        }
    }

    public class BrowseViewModel
    {
        public FilterViewModel Filter { get; set; }
        public List<BalProgrammer> Users { get; set; }
    }
}