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
        private readonly IPropertyMap<User, DalUser> map;

        public UserRepository(DbContext context)
        {
            this.context = context;
            set = context.Set<User>();
        }

        public DalUser GetById(int id)
        {
            return set.Find(id).ToDal();
        }

        public IEnumerable<DalUser> GetAll()
        {
            return set.AsQueryable().Select(e => e.ToDal());
        }

        public IEnumerable<DalUser> Get(Expression<Func<DalUser, bool>> predicate)
        {
            if (map == null)
                InitializeMap();

            return set.Where(map.MapExpression(predicate)).Select(e => e.ToDal());
        }

        public void Create(DalUser entity)
        {
            set.Add(entity.ToDb());
            context.Set<UserInfo>().Add(new UserInfo { Id = entity.Id });
        }

        public void Update(DalUser entity)
        {
            User user = entity.ToDb();
            set.Attach(user);
            context.Entry(user).State = EntityState.Modified;

            if (entity is DalProgrammer)
            {
                UserInfo ui = ((DalProgrammer)entity).GetUserInfo();
                context.Set<UserInfo>().Attach(ui);
                context.Entry(ui).State = EntityState.Modified;

                UpdateSkills((DalProgrammer)entity);
            }
            if (entity is DalManager)
                UpdateFilters((DalManager)entity);
        }

        public void Delete(int id)
        {
            Delete(set.Find(id));
        }

        public void Delete(DalUser entity)
        {
            Delete(entity.ToDb());
        }

        private void Delete(User entity)
        {
            set.Remove(entity);
        }

        private void UpdateSkills(DalProgrammer user)
        {
            DbSet<UserSkill> skillSet = context.Set<UserSkill>();
            skillSet.RemoveRange(set.Find(user.Id).UserSkills);

            foreach (var item in user.Skills)
            {
                UserSkill skill = new UserSkill
                {
                    UserId = user.Id,
                    SkillId = item.Key.Id,
                    Level = item.Value
                };
                skillSet.Add(skill);
            }
        }

        private void UpdateFilters(DalManager dalManager)
        {
            DbSet<Filter> filterSet = context.Set<Filter>();
            filterSet.RemoveRange(set.Find(dalManager.Id).Filters);

            foreach (var item in ((DalManager)dalManager).Filters.Select(f => f.ToDb(dalManager.Id)))
            {
                filterSet.Add(item);
            }
        }

        private void InitializeMap()
        {
            map.Map(d => d.Id, e => e.Id)
                .Map(d => d.Email, e => e.Email)
                .Map(d => d.Password, e => e.Password)
                .Map(d => d.Role.Id, e => e.RoleId)
                .Map(d => d.Role.Name, e => e.Role.Name);
        }
    }
}
