using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BP.DAL.Interface.Repositories;
using BP.ORM;
using System.Data.Entity;
using System.Linq.Expressions;

namespace BP.DAL.Concrete
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext context;
        private readonly DbSet<T> set;

        public Repository(DbContext dbContext)
        {
            context = dbContext;
            set = context.Set<T>();
        }

        public T GetById(int id)
        {
            return set.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return set.AsEnumerable();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return set.Where(predicate);
        }

        public void Create(T entity)
        {
            set.Add(entity);
        }

        public void Update(T entity)
        {
            set.Attach(entity);
            context.Entry<T>(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Delete(set.Find(id));
        }

        public void Delete(T entity)
        {
            if (context.Entry<T>(entity).State == EntityState.Detached)
                set.Attach(entity);
            set.Remove(entity);
        }
    }
}
