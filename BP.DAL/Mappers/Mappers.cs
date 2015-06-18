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
    public abstract class MapperBase<TEntity, TDal> : IMapper<TEntity, TDal>
        where TEntity : class, new()
        where TDal : class, IEntity
    {
        protected readonly PropertyMap<TEntity, TDal> propertyMap;

        protected MapperBase()
        {
            propertyMap = new PropertyMap<TEntity, TDal>();
        }

        public virtual TEntity ToDb(TDal obj)
        {
            if (obj == null)
                return null;

            TEntity entity = new TEntity();
            CopyFields(obj, entity);
            return entity;
        }

        public abstract void CopyFields(TDal obj, TEntity entity);

        public abstract TDal ToDal(TEntity entity);

        public virtual Expression<Func<TEntity, bool>> MapExpression(Expression<Func<TDal, bool>> sourceExpression)
        {
            return propertyMap.MapExpression(sourceExpression);
        }
    }

    public class UserMapper : MapperBase<User, DalUser>
    {
        public UserMapper()
        {
            propertyMap.Map(d => d.Id, e => e.Id)
                .Map(d => d.Email, e => e.Email)
                .Map(d => d.Password, e => e.Password)
                .Map(d => d.Role.Id, e => e.RoleId)
                .Map(d => d.Role.Name, e => e.Role.Name);
        }

        public override void CopyFields(DalUser dalFrom, User dbTo)
        {
            if (dbTo == null || dalFrom == null)
                return;

            dbTo.Id = dalFrom.Id;
            dbTo.Email = dalFrom.Email;
            dbTo.Password = dalFrom.Password;
            dbTo.RoleId = dalFrom.Role.Id;

            if (dalFrom is DalProgrammer)
            {
                DalProgrammer programmer = (DalProgrammer)dalFrom;
                if (dbTo.UserInfo == null)
                    dbTo.UserInfo = new UserInfo();

                dbTo.UserInfo.About = programmer.About;
                dbTo.UserInfo.BirthDate = programmer.BirthDate;
                dbTo.UserInfo.Id = programmer.Id;
                dbTo.UserInfo.ImageType = programmer.ImapeType;
                dbTo.UserInfo.Name = programmer.Name;
                dbTo.UserInfo.Photo = programmer.Photo;
            }
        }

        public override DalUser ToDal(User entity)
        {
            if (entity == null)
                return null;

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
    }

    public class SkillMapper : MapperBase<Skill, DalSkill>
    {
        public SkillMapper()
        {
            propertyMap.Map(d => d.Id, e => e.Id)
                .Map(d => d.Name, e => e.Name);
        }

        public override void CopyFields(DalSkill dalFrom, Skill dbTo)
        {
            if (dbTo == null || dalFrom == null)
                return;

            dbTo.Id = dalFrom.Id;
            dbTo.Name = dalFrom.Name;
        }

        public override DalSkill ToDal(Skill entity)
        {
            if (entity == null)
                return null;

            return new DalSkill { Id = entity.Id, Name = entity.Name };
        }
    }

    public class FilterMapper : MapperBase<Filter, DalFilter>
    {
        private readonly BinaryFormatter binaryFormatter;

        public FilterMapper()
        {
            binaryFormatter = new BinaryFormatter();
            propertyMap.Map(d => d.Id, e => e.Id)
                .Map(d => d.LastViewed, e => e.LastViewed)
                .Map(d => d.UserId, e => e.UserId);
        }

        public override void CopyFields(DalFilter dalFrom, Filter dbTo)
        {
            if (dbTo == null || dalFrom == null)
                return;

            using (MemoryStream stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, dalFrom.Skills);
                dbTo.Id = dalFrom.Id;
                dbTo.UserId = dalFrom.UserId;
                dbTo.LastViewed = dalFrom.LastViewed;
                dbTo.Skills = stream.ToArray();
            }
        }

        public override DalFilter ToDal(Filter entity)
        {
            if (entity == null)
                return null;

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
    }

    public class RoleMapper : MapperBase<Role, DalRole>
    {
        public RoleMapper()
        {
            propertyMap.Map(d => d.Id, e => e.Id)
                .Map(d => d.Name, e => e.Name);
        }

        public override void CopyFields(DalRole dalFrom, Role dbTo)
        {
            if (dbTo == null || dalFrom == null)
                return;

            dbTo.Id = dalFrom.Id;
            dbTo.Name = dalFrom.Name;
        }

        public override DalRole ToDal(Role entity)
        {
            if (entity == null)
                return null;
            return new DalRole { Id = entity.Id, Name = entity.Name };
        }
    }

    public class UserSkillMapper : MapperBase<UserSkill, DalUserSkill>
    {
        private readonly IMapper<Skill, DalSkill> skillMapper;
        private readonly IMapper<User, DalUser> userMapper;

        public UserSkillMapper(IMapper<Skill, DalSkill> skillMapper, IMapper<User, DalUser> userMapper)
        {
            propertyMap.Map(d => d.User.Id, e => e.UserId)
                .Map(d => d.Skill.Id, e => e.SkillId)
                .Map(d => d.Level, e => e.Level);
            this.skillMapper = skillMapper;
            this.userMapper = userMapper;
        }

        public override void CopyFields(DalUserSkill dalFrom, UserSkill dbTo)
        {
            if (dbTo == null || dalFrom == null)
                return;

            dbTo.UserId = dalFrom.User.Id;
            dbTo.SkillId = dalFrom.Skill.Id;
            dbTo.Level = dalFrom.Level;
        }

        public override DalUserSkill ToDal(UserSkill entity)
        {
            if (entity == null)
                return null;

            return new DalUserSkill
            {
                Level = entity.Level,
                Skill = skillMapper.ToDal(entity.Skill),
                User = (DalProgrammer)userMapper.ToDal(entity.User)
            };
        }
    }
}
