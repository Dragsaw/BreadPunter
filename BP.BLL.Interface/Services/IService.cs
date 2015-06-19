using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BP.BLL.Interface.Entities;

namespace BP.BLL.Interface.Services
{
    public interface IService<T> : IDisposable where T : class, IBllEntity
    {
        T Find(int id);
        T Find(string uniqueKey);
        IEnumerable<T> GetAll();
        void Create(T entity);
        void Update(T entity);
        bool Remove(int id);
        bool Remove(T entity);
        bool Remove(string uniqueKey);
    }
}
