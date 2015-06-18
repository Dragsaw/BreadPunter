using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Entities.Users;
using BP.DAL.Interface.Entities;
using BP.DAL.Interface.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.BLL.Mappers
{
    public static class Mappers
    {
        #region BAL to DAL mappers
        public static DalUser ToDal(this BalUser user)
        {
            if (user == null)
                return null;

            DalUser dalUser = InitializeDalUserProperties(user);
            dalUser.Id = user.Id;
            dalUser.Email = user.Email;
            dalUser.Password = user.Password;
            dalUser.Role = user.Role.ToDal();

            return dalUser;
        }

        public static DalRole ToDal(this BalRole role)
        {
            if (role == null)
                return null;

            return new DalRole
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public static DalSkill ToDal(this BalSkill skill)
        {
            if (skill == null)
                return null;

            return new DalSkill
            {
                Id = skill.Id,
                Name = skill.Name
            };
        }

        public static DalFilter ToDal(this BalFilter filter)
        {
            if (filter == null)
                return null;

            return new DalFilter
            {
                Id = filter.Id,
                Skills = filter.Skills.ToDictionary(k => k.Key.ToDal(), v => v.Value),
                LastViewed = filter.LastViewed
            };
        }

        public static DalUserSkill ToDal(this BalUserSkill userSkill)
        {
            if (userSkill == null)
                return null;

            return new DalUserSkill
            {
                User = (DalProgrammer)userSkill.User.ToDal(),
                Skill = userSkill.Skill.ToDal(),
                Level = userSkill.Level
            };
        }
        #endregion

        #region DAL to BAL mappers
        public static BalUser ToBal(this DalUser user)
        {
            if (user == null)
                return null;

            BalUser balUser = InitializeBalUserProperties(user);
            balUser.Id = user.Id;
            balUser.Email = user.Email;
            balUser.Password = user.Password;
            balUser.Role = user.Role.ToBal();

            return balUser;
        }

        public static BalRole ToBal(this DalRole role)
        {
            return new BalRole
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public static BalSkill ToBal(this DalSkill skill)
        {
            return new BalSkill
            {
                Id = skill.Id,
                Name = skill.Name
            };
        }

        public static BalFilter ToBal(this DalFilter filter)
        {
            return new BalFilter
            {
                Id = filter.Id,
                Skills = filter.Skills.ToDictionary(k => k.Key.ToBal(), v => v.Value),
                LastViewed = filter.LastViewed
            };
        }

        public static BalUserSkill ToBal(this DalUserSkill skill)
        {
            return new BalUserSkill
            {
                Skill = skill.Skill.ToBal(),
                User = (BalProgrammer)skill.User.ToBal(),
                Level = skill.Level
            };
        }
        #endregion

        private static BalUser InitializeBalUserProperties(DalUser user)
        {
            BalUser balUser;
            if (user is DalAdmin)
                balUser = new BalAdmin();
            else if (user is DalProgrammer)
            {
                DalProgrammer dalProgrammer = (DalProgrammer)user;
                var skillsDict = dalProgrammer.Skills.ToDictionary(k => k.Skill.ToBal(), v => v.Level);
                balUser = new BalProgrammer()
                {
                    Name = dalProgrammer.Name,
                    About = dalProgrammer.About,
                    BirthDate = dalProgrammer.BirthDate,
                    Photo = dalProgrammer.Photo,
                    ImageType = dalProgrammer.ImapeType,
                    Skills = skillsDict
                };
            }
            else if (user is DalManager)
                balUser = new BalManager
                {
                    Filters = ((DalManager)user).Filters.Select(f => f.ToBal()).ToList()
                };
            else balUser = new BalUser();

            return balUser;
        }

        private static DalUser InitializeDalUserProperties(BalUser user)
        {
            DalUser dalUser;
            if (user is BalAdmin)
                dalUser = new DalAdmin();
            else if (user is BalProgrammer)
            {
                BalProgrammer programmer = (BalProgrammer)user;
                DalProgrammer dalProgrammer = new DalProgrammer();
                dalProgrammer.Name = programmer.Name;
                dalProgrammer.About = programmer.About;
                dalProgrammer.BirthDate = programmer.BirthDate;
                dalProgrammer.Photo = programmer.Photo;
                dalProgrammer.ImapeType = programmer.ImageType;
                dalProgrammer.Skills = programmer.Skills.Select(x => new DalUserSkill
                {
                    User = dalProgrammer,
                    Level = x.Value,
                    Skill = x.Key.ToDal()
                });
                dalUser = dalProgrammer;
            }
            else if (user is BalManager)
                dalUser = new DalManager()
                {
                    Filters = ((BalManager)user).Filters.Select(f => f.ToDal())
                };
            else dalUser = new DalUser();

            return dalUser;
        }
    }
}
