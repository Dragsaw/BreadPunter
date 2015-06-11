﻿using BP.DAL.Interface.Entities;
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
    public class SkillRepository : IRepository<DalSkill>
    {
        private readonly DbContext context;
        private readonly DbSet<Skill> set;
        private IPropertyMap<Skill, DalSkill> map;

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

        public IEnumerable<DalSkill> Get(Expression<Func<DalSkill, bool>> predicate)
        {
            if (map == null)
                InitializeMap();

            return set.Where(map.MapExpression(predicate)).Select(e => e.ToDal());
        }

        private void InitializeMap()
        {
            map = new PropertyMap<Skill, DalSkill>();
            map.Map(d => d.Id, e => e.Id)
                .Map(d => d.Name, e => e.Name);
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
