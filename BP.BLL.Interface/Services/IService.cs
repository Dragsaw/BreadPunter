using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BP.BLL.Interface.Entities;

namespace BP.BLL.Interface.Services
{
    public interface IService<T> where T : class, IBalEntity
    {
        T Find(int id);
        IEnumerable<T> GetAll();
        void Create(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
