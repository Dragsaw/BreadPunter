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
        public static DalUser ToDal(this BllUser user)
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

        public static DalRole ToDal(this BllRole role)
        {
            if (role == null)
                return null;

            return new DalRole
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public static DalSkill ToDal(this BllSkill skill)
        {
            if (skill == null)
                return null;

            return new DalSkill
            {
                Id = skill.Id,
                Name = skill.Name
            };
        }

        public static DalFilter ToDal(this BllFilter filter, int userId)
        {
            if (filter == null)
                return null;

            return new DalFilter
            {
                Id = filter.Id,
                UserId = userId,
                Skills = filter.Skills.ToDictionary(k => k.Key.ToDal(), v => v.Value),
                LastViewed = filter.LastViewed
            };
        }

        public static DalUserSkill ToDal(this BllUserSkill userSkill)
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
        public static BllUser ToBal(this DalUser user)
        {
            if (user == null)
                return null;

            BllUser balUser = InitializeBalUserProperties(user);
            balUser.Id = user.Id;
            balUser.Email = user.Email;
            balUser.Password = user.Password;
            balUser.Role = user.Role.ToBal();

            return balUser;
        }

        public static BllRole ToBal(this DalRole role)
        {
            if (role == null)
                return null;

            return new BllRole
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public static BllSkill ToBal(this DalSkill skill)
        {
            if (skill == null)
                return null;

            return new BllSkill
            {
                Id = skill.Id,
                Name = skill.Name
            };
        }

        public static BllFilter ToBal(this DalFilter filter)
        {
            return new BllFilter
            {
                Id = filter.Id,
                Skills = filter.Skills.ToDictionary(k => k.Key.ToBal(), v => v.Value),
                LastViewed = filter.LastViewed
            };
        }

        public static BllUserSkill ToBal(this DalUserSkill skill)
        {
            return new BllUserSkill
            {
                Skill = skill.Skill.ToBal(),
                User = (BllProgrammer)skill.User.ToBal(),
                Level = skill.Level
            };
        }
        #endregion

        private static BllUser InitializeBalUserProperties(DalUser user)
        {
            BllUser balUser;
            if (user is DalAdmin)
                balUser = new BllAdmin();
            else if (user is DalProgrammer)
            {
                DalProgrammer dalProgrammer = (DalProgrammer)user;
                var skillsDict = dalProgrammer.Skills.ToDictionary(k => k.Skill.ToBal(), v => v.Level);
                balUser = new BllProgrammer()
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
                balUser = new BllManager
                {
                    Filters = ((DalManager)user).Filters.Select(f => f.ToBal()).ToList()
                };
            else balUser = new BllUser();

            return balUser;
        }

        private static DalUser InitializeDalUserProperties(BllUser user)
        {
            DalUser dalUser;
            if (user is BllAdmin)
                dalUser = new DalAdmin();
            else if (user is BllProgrammer)
            {
                BllProgrammer programmer = (BllProgrammer)user;
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
            else if (user is BllManager)
                dalUser = new DalManager()
                {
                    Filters = ((BllManager)user).Filters.Select(f => f.ToDal(user.Id))
                };
            else dalUser = new DalUser();

            return dalUser;
        }
    }
}
