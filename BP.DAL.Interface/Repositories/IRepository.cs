using BP.DAL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BP.DAL.Interface.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Find(int id);
        T Find(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        void Create(T entity);
        void Update(T entity);
        void Remove(int id);
        void Remove(T entity);
    }
}
