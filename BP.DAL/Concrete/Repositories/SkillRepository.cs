using BP.DAL.Interface.Entities;
using BP.DAL.Interface.Repositories;
using BP.ORM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BP.DAL.Mappers;

namespace BP.DAL.Concrete.Repositories
{
    public class SkillRepository : IRepository<DalSkill>
    {
        private readonly DbContext context;
        private readonly DbSet<Skill> set;

        public SkillRepository(DbContext context)
        {
            this.context = context;
            set = context.Set<Skill>();
        }

        public DalSkill GetById(int id)
        {
            return set.Find(id).ToDal();
        }

        public IEnumerable<DalSkill> GetAll()
        {
            return set.Select(s => s.ToDal());
        }

        public IEnumerable<DalSkill> Get(System.Linq.Expressions.Expression<Func<DalSkill, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Create(DalSkill entity)
        {
            set.Add(entity.ToDb());
        }

        public void Update(DalSkill entity)
        {
            Skill skill = entity.ToDb();
            set.Attach(skill);
            context.Entry<Skill>(skill).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(DalSkill entity)
        {
            throw new NotImplementedException();
        }
    }
}
