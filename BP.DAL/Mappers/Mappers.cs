using BP.DAL.Interface.Entities;
using BP.DAL.Interface.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BP.ORM;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace BP.DAL.Mappers
{
    public static class Mappers
    {
        private static readonly BinaryFormatter binaryFormatter;

        static Mappers()
        {
            binaryFormatter = new BinaryFormatter();
        }

        #region Mappers for ORM to DAL entities
        public static DalUser ToDal(this User user)
        {
            DalUser dalUser = InitializeDalUserProperties(user);
            dalUser.Id = user.Id;
            dalUser.Email = user.Email;
            dalUser.Password = user.Password;
            dalUser.Role = user.Role.ToDal();
            return dalUser;
        }

        public static DalSkill ToDal(this Skill skill)
        {
            return new DalSkill
            {
                Id = skill.Id,
                Name = skill.Name
            };
        }

        public static DalFilter ToDal(this Filter filter)
        {
            using (Stream stream = new MemoryStream(filter.Skills))
            {
                return new DalFilter
                {
                    Id = filter.Id,
                    LastViewed = filter.LastViewed,
                    Skills = (IDictionary<DalSkill, int>)binaryFormatter.Deserialize(stream)
                };
            }
        }

        public static DalRole ToDal(this Role role)
        {
            return new DalRole
            {
                Id = role.Id,
                Name = role.Name
            };
        }
        #endregion

        #region Mappers for DAL to ORM entities
        public static User ToDb(this DalUser user)
        {
            User dbUser = InitializeUserProperties(user);
            dbUser.Id = user.Id;
            dbUser.Email = user.Email;
            dbUser.Password = user.Password;
            dbUser.RoleId = user.Role.Id;

            return dbUser;
        }

        public static Skill ToDb(this DalSkill skill)
        {
            return new Skill
            {
                Id = skill.Id,
                Name = skill.Name
            };
        }

        public static Filter ToDb(this DalFilter filter, int userId)
        {
            Filter f = new Filter
            {
                Id = filter.Id,
                UserId = userId,
                LastViewed = filter.LastViewed,
            };
            using (MemoryStream stream = new MemoryStream(f.Skills))
                binaryFormatter.Serialize(stream, filter.Skills);
            return f;
        }

        public static Role ToDb(this DalRole role)
        {
            return new Role
            {
                Id = role.Id,
                Name = role.Name
            };
        }
        #endregion

        #region Additional methods
        public static UserInfo GetUserInfo(this DalProgrammer programmer)
        {
            return new UserInfo
            {
                About = programmer.About,
                BirthDate = programmer.BirthDate,
                Name = programmer.Name,
                Photo = programmer.Photo
            };
        }

        private static DalUser InitializeDalUserProperties(User user)
        {
            DalUser dalUser;
            switch (user.Role.Name)
            {
                case ("Admin"):
                    dalUser = new DalAdmin();
                    break;
                case ("Programmer"):
                    DalProgrammer programmer = new DalProgrammer();
                    programmer.Name = user.UserInfo.Name;
                    programmer.About = user.UserInfo.About;
                    programmer.BirthDate = user.UserInfo.BirthDate;
                    programmer.Photo = user.UserInfo.Photo;
                    programmer.Skills = user.UserSkills.ToDictionary(x => x.Skill.ToDal(), x => x.Level);
                    dalUser = programmer;
                    break;
                case ("Manager"):
                    dalUser = new DalManager();
                    ((DalManager)dalUser).Filters = user.Filters.Select(f => f.ToDal());
                    break;
                default:
                    dalUser = new DalUser();
                    break;
            }
            return dalUser;
        }

        private static User InitializeUserProperties(DalUser user)
        {
            User dbUser = new User();
            switch (user.Role.Name)
            {
                case ("Programmer"):
                    dbUser.UserInfo = ((DalProgrammer)user).GetUserInfo();
                    break;
                case ("Manager"):
                    dbUser.Filters = ((DalManager)user).Filters.Select(f => f.ToDb(user.Id)).ToList();
                    break;
                default:
                    break;
            }
            return dbUser;
        }
        #endregion
    }
}
