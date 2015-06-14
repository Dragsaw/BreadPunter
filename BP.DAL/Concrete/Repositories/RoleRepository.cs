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
using BP.DAL.Interface.Mappers;

namespace BP.DAL.Concrete.Repositories
{
    public class RoleRepository : IRepository<DalRole>
    {
        private readonly DbContext context;
        private readonly DbSet<Role> set;
        private static IPropertyMap<Role, DalRole> map;

        public RoleRepository(DbContext context)
        {
            this.context = context;
            set = context.Set<Role>();
        }

        public DalRole Find(int id)
        {
            return set.Find(id).ToDal();
        }

        public DalRole Find(System.Linq.Expressions.Expression<Func<DalRole, bool>> predicate)
        {
            if (map == null)
                InitializeMap();
            return set.FirstOrDefault(map.MapExpression(predicate)).ToDal();
        }

        public IEnumerable<DalRole> GetAll()
        {
            return set.AsEnumerable().Select(x => x.ToDal());
        }

        public IEnumerable<DalRole> Get(System.Linq.Expressions.Expression<Func<DalRole, bool>> predicate)
        {
            if (map == null)
                InitializeMap();
            return set.Where(map.MapExpression(predicate)).AsEnumerable().Select(r => r.ToDal());
        }

        public void Create(DalRole entity)
        {
            set.Add(entity.ToDb());
        }

        public void Update(DalRole entity)
        {
            Role role = set.Find(entity.Id);
            if (role != null)
            {
                role = entity.ToDb();
                set.Attach(role);
                context.Entry(role).State = EntityState.Modified;
            }
        }

        public void Remove(int id)
        {
            Remove(Find(id));
        }

        public void Remove(DalRole entity)
        {
            Remove(entity.Id);
        }

        private void Remove(Role role)
        {
            if (role != null)
                set.Remove(role);
        }

        private static void InitializeMap()
        {
            map = new PropertyMap<Role, DalRole>();
            map.Map(d => d.Id, e => e.Id)
                .Map(d => d.Name, e => e.Name);
        }
    }
}
