using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Entities.Users;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Globalization;

namespace BP.WebUI.Models
{
    public class FilterViewModel
    {
        public int Id { get; set; }
        public List<UserSkillViewModel> Skills { get; set; }
        public DateTime? LastViewed { get; set; }

        public static string ToJson(FilterViewModel obj)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        public static FilterViewModel ToObject(string json)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<FilterViewModel>(json);
        }
    }

    public class UserSkillViewModel
    {
        public BllSkill Skill { get; set; }
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

        public void GetInfo(BllProgrammer user)
        {
            Name = user.Name;
            About = user.About;
            ImageType = user.ImageType;
            if (user.BirthDate != null)
                BirthDate = ((DateTime)user.BirthDate).ToShortDateString();
        }

        public void SetUserInfo(BllProgrammer user)
        {
            user.Name = Name;
            user.About = About;
            if (Image != null)
            {
                user.ImageType = Image.ContentType;
                user.Photo = new byte[Image.ContentLength];
                Image.InputStream.Read(user.Photo, 0, Image.ContentLength);
            }
        }
    }

    public class BrowseViewModel
    {
        public FilterViewModel Filter { get; set; }
        public List<BllProgrammer> Users { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }
        public IEnumerable<int> IncludedSkillsId
        {
            get
            {
                return Filter.Skills.Where(x => x.Level > 0).Select(x => x.Skill.Id);
            }
        }
    }
}