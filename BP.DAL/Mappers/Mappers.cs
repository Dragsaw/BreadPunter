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
using BP.DAL.Interface.Mappers;
using System.Linq.Expressions;

namespace BP.DAL.Mappers
{
    public class UserMapper : IMapper<User, DalUser>
    {
        private readonly PropertyMap<User, DalUser> propertyMap;

        public UserMapper()
        {
            propertyMap = new PropertyMap<User, DalUser>();
            propertyMap.Map(d => d.Id, e => e.Id)
                .Map(d => d.Email, e => e.Email)
                .Map(d => d.Password, e => e.Password)
                .Map(d => d.Role.Id, e => e.RoleId)
                .Map(d => d.Role.Name, e => e.Role.Name);
        }

        public User ToDb(DalUser obj)
        {
            User dbUser = new User();
            dbUser.Id = obj.Id;
            dbUser.Email = obj.Email;
            dbUser.Password = obj.Password;
            dbUser.RoleId = obj.Role.Id;

            if (obj is DalProgrammer)
            {
                DalProgrammer programmer = (DalProgrammer)obj;
                if (dbUser.UserInfo == null)
                    dbUser.UserInfo = new UserInfo();

                dbUser.UserInfo.About = programmer.About;
                dbUser.UserInfo.BirthDate = programmer.BirthDate;
                dbUser.UserInfo.Id = programmer.Id;
                dbUser.UserInfo.ImageType = programmer.ImapeType;
                dbUser.UserInfo.Name = programmer.Name;
                dbUser.UserInfo.Photo = programmer.Photo;
            }

            return dbUser;
        }

        public DalUser ToDal(User entity)
        {
            DalUser user = new DalUser();
            string role = entity.Role.Name;

            if (role == "Admin")
                user = new DalAdmin();
            else if (role == "Manager")
                user = new DalManager();
            else if (role == "Programmer")
            {
                DalProgrammer programmer = new DalProgrammer();
                if (entity.UserInfo != null)
                {
                    programmer.About = entity.UserInfo.About;
                    programmer.BirthDate = entity.UserInfo.BirthDate;
                    programmer.ImapeType = entity.UserInfo.ImageType;
                    programmer.Name = entity.UserInfo.Name;
                    programmer.Photo = entity.UserInfo.Photo;
                }
                user = programmer;
            }

            user.Id = entity.Id;
            user.Email = entity.Email;
            user.Password = entity.Password;
            user.Role = new DalRole { Id = entity.RoleId, Name = role };

            return user;
        }

        public Expression<Func<User, bool>> MapExpression(Expression<Func<DalUser, bool>> sourceExpression)
        {
            return propertyMap.MapExpression(sourceExpression);
        }
    }

    public class SkillMapper : IMapper<Skill, DalSkill>
    {
        private readonly PropertyMap<Skill, DalSkill> propertyMap;

        public SkillMapper()
        {
            propertyMap = new PropertyMap<Skill, DalSkill>();
            propertyMap.Map(d => d.Id, e => e.Id)
                .Map(d => d.Name, e => e.Name);
        }

        public Skill ToDb(DalSkill obj)
        {
            return new Skill { Id = obj.Id, Name = obj.Name };
        }

        public DalSkill ToDal(Skill entity)
        {
            return new DalSkill { Id = entity.Id, Name = entity.Name };
        }

        public Expression<Func<Skill, bool>> MapExpression(Expression<Func<DalSkill, bool>> sourceExpression)
        {
            return propertyMap.MapExpression(sourceExpression);
        }
    }

    public class FilterMapper : IMapper<Filter, DalFilter>
    {
        private readonly BinaryFormatter binaryFormatter;
        private readonly PropertyMap<Filter, DalFilter> propertyMap;

        public FilterMapper()
        {
            binaryFormatter = new BinaryFormatter();
            propertyMap = new PropertyMap<Filter, DalFilter>();
            propertyMap.Map(d => d.Id, e => e.Id)
                .Map(d => d.LastViewed, e => e.LastViewed)
                .Map(d => d.UserId, e => e.UserId);
        }

        public Filter ToDb(DalFilter obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, obj.Skills);
                Filter filter = new Filter
                {
                    Id = obj.Id,
                    UserId = obj.UserId,
                    LastViewed = obj.LastViewed,
                    Skills = stream.ToArray()
                };
                return filter;
            }
        }

        public DalFilter ToDal(Filter entity)
        {
            using (Stream stream = new MemoryStream(entity.Skills))
            {
                return new DalFilter
                {
                    Id = entity.Id,
                    UserId = entity.UserId,
                    LastViewed = entity.LastViewed,
                    Skills = (IDictionary<DalSkill, int>)binaryFormatter.Deserialize(stream)
                };
            }
        }

        public Expression<Func<Filter, bool>> MapExpression(Expression<Func<DalFilter, bool>> sourceExpression)
        {
            return propertyMap.MapExpression(sourceExpression);
        }
    }

    public class RoleMapper : IMapper<Role, DalRole>
    {
        private readonly PropertyMap<Role, DalRole> propertyMap;

        public RoleMapper()
        {
            propertyMap = new PropertyMap<Role, DalRole>();
            propertyMap.Map(d => d.Id, e => e.Id)
                .Map(d => d.Name, e => e.Name);
        }

        public Role ToDb(DalRole obj)
        {
            return new Role { Id = obj.Id, Name = obj.Name };
        }

        public DalRole ToDal(Role entity)
        {
            return new DalRole { Id = entity.Id, Name = entity.Name };
        }

        public Expression<Func<Role, bool>> MapExpression(Expression<Func<DalRole, bool>> sourceExpression)
        {
            return propertyMap.MapExpression(sourceExpression);
        }
    }

    public class UserSkillMapper : IMapper<UserSkill, DalUserSkill>
    {
        private readonly PropertyMap<UserSkill, DalUserSkill> propertyMap;
        private readonly IMapper<Skill, DalSkill> skillMapper;
        private readonly IMapper<User, DalUser> userMapper;

        public UserSkillMapper(IMapper<Skill, DalSkill> skillMapper, IMapper<User, DalUser> userMapper)
        {
            propertyMap = new PropertyMap<UserSkill, DalUserSkill>();
            propertyMap.Map(d => d.User.Id, e => e.UserId)
                .Map(d => d.Skill.Id, e => e.SkillId)
                .Map(d => d.Level, e => e.Level);
            this.skillMapper = skillMapper;
            this.userMapper = userMapper;
        }

        public UserSkill ToDb(DalUserSkill obj)
        {
            return new UserSkill
            {
                UserId = obj.User.Id,
                SkillId = obj.Skill.Id,
                Level = obj.Level
            };
        }

        public DalUserSkill ToDal(UserSkill entity)
        {
            return new DalUserSkill
            {
                Level = entity.Level,
                Skill = skillMapper.ToDal(entity.Skill),
                User = (DalProgrammer)userMapper.ToDal(entity.User)
            };
        }

        public Expression<Func<UserSkill, bool>> MapExpression(Expression<Func<DalUserSkill, bool>> sourceExpression)
        {
            return propertyMap.MapExpression(sourceExpression);
        }
    }
}
