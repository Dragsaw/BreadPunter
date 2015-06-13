using BP.DAL.Interface.Entities;
using BP.DAL.Interface.Mappers;
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

namespace BP.DAL.Concrete.Repositories
{
    public class UserSkillRepository : IRepository<DalUserSkill>
    {
        private readonly DbContext context;
        private readonly DbSet<UserSkill> set;
        private IPropertyMap<UserSkill, DalUserSkill> map;

        public UserSkillRepository(DbContext context)
        {
            this.context = context;
            set = context.Set<UserSkill>();
        }

        public DalUserSkill GetById(int id)
        {
            int userId = int.Parse(id.ToString().Substring(0, 5));
            int skillId = int.Parse(id.ToString().Substring(5));
            return set.Find(userId, skillId).ToDal();
        }

        public IEnumerable<DalUserSkill> GetAll()
        {
            return set.AsEnumerable().Select(e => e.ToDal());
        }

        public IEnumerable<DalUserSkill> Get(Expression<Func<DalUserSkill, bool>> predicate)
        {
            if (map == null)
                InitializeMap();
            return set.Where(map.MapExpression(predicate)).AsEnumerable().Select(e => e.ToDal());
        }

        public void Create(DalUserSkill entity)
        {
            set.Add(entity.ToDb());
        }

        public void Update(DalUserSkill entity)
        {
            UserSkill dbEntity = entity.ToDb();
            set.Attach(dbEntity);
            context.Entry(dbEntity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Delete(GetById(id));
        }

        public void Delete(DalUserSkill entity)
        {
            set.Remove(entity.ToDb());
        }

        private void InitializeMap()
        {
            map = new PropertyMap<UserSkill, DalUserSkill>();
            map.Map(d => d.User.Id, e => e.UserId)
                .Map(d => d.Skill.Id, e => e.SkillId)
                .Map(d => d.Level, e => e.Level);
        }
    }
}
