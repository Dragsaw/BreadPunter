using BP.DAL.Interface.Entities.Users;
using BP.DAL.Interface.Repositories;
using BP.ORM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BP.DAL.Mappers;
using System.Linq.Expressions;
using BP.DAL.Interface.Mappers;

namespace BP.DAL.Concrete.Repositories
{
    public class UserRepository : IRepository<DalUser>
    {
        private readonly DbContext context;
        private readonly DbSet<User> set;
        private static IPropertyMap<User, DalUser> map;

        public UserRepository(DbContext context)
        {
            this.context = context;
            set = context.Set<User>();
        }

        public DalUser Find(int id)
        {
            return set.Find(id).ToDal();
        }

        public DalUser Find(Expression<Func<DalUser, bool>> predicate)
        {
            if (map == null)
                InitializeMap();
            User user = set.FirstOrDefault(map.MapExpression(predicate));
            return user == null ? null : user.ToDal();
        }

        public IEnumerable<DalUser> GetAll()
        {
            return set.AsEnumerable().Select(e => e.ToDal());
        }

        public IEnumerable<DalUser> Get(Expression<Func<DalUser, bool>> predicate)
        {
            if (map == null)
                InitializeMap();

            return set.Where(map.MapExpression(predicate)).AsEnumerable().Select(e => e.ToDal());
        }

        public void Create(DalUser entity)
        {
            set.Add(entity.ToDb());
            context.Set<UserInfo>().Add(new UserInfo { Id = entity.Id });
        }

        public void Update(DalUser entity)
        {
            User user = entity.ToDb();
            //set.Attach(user);
            //context.Entry(user).State = EntityState.Modified;

            if (entity is DalProgrammer)
            {
                UserInfo ui = ((DalProgrammer)entity).GetUserInfo();
                //context.Set<UserInfo>().Attach(ui);
                //context.Entry(ui).State = EntityState.Modified;

                UpdateSkills((DalProgrammer)entity);
            }
            if (entity is DalManager)
                UpdateFilters((DalManager)entity);
        }

        public void Remove(int id)
        {
            User user = set.Find(id);
            set.Remove(user);
        }

        public void Remove(DalUser entity)
        {
            Remove(entity.Id);
        }

        private void UpdateSkills(DalProgrammer user)
        {
            DbSet<UserSkill> skillSet = context.Set<UserSkill>();
            var userSkills = user.Skills.Select(x => new UserSkill
            {
                UserId = user.Id,
                SkillId = x.Key.Id,
                Level = x.Value
            });
            var skillsToAdd = user.Skills.Where(x => !userSkills.Any(y => y.SkillId == x.Key.Id));
            var skillsInDb = skillSet.Where(x => x.UserId == user.Id);

            foreach (var skill in userSkills)
            {
                var skillInDb = skillsInDb.FirstOrDefault(x => x.SkillId == skill.SkillId);
                if (skillInDb == null)
                {
                    skillSet.Add(skill);
                }
                else skillInDb.Level = skill.Level;
            }

            foreach (var skill in skillsInDb)
            {
                if (!userSkills.Any(x => x.SkillId == skill.SkillId))
                    skillSet.Remove(skill);
            }
        }

        private void UpdateFilters(DalManager dalManager)
        {
            DbSet<Filter> filterSet = context.Set<Filter>();
            var userFilters = dalManager.Filters.Select(x => x.ToDb(dalManager.Id));
            var filtersInDb = filterSet.Where(x => x.UserId == dalManager.Id);

            foreach (var filter in userFilters)
            {
                var filterInDb = filtersInDb.FirstOrDefault(x => x.Id == filter.Id);
                if (filterInDb == null)
                    filterSet.Add(filter);
                else
                {
                    filterInDb.LastViewed = filter.LastViewed;
                    filterInDb.Skills = filter.Skills;
                }
            }

            foreach (var filter in filtersInDb)
            {
                if (!userFilters.Any(x => x.Id == filter.Id))
                    filterSet.Remove(filter);
            }
        }

        private static void InitializeMap()
        {
            map = new PropertyMap<User, DalUser>();
            map.Map(d => d.Id, e => e.Id)
                .Map(d => d.Email, e => e.Email)
                .Map(d => d.Password, e => e.Password)
                .Map(d => d.Role.Id, e => e.RoleId)
                .Map(d => d.Role.Name, e => e.Role.Name);
        }
    }
}
