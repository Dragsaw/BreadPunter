using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BP.DAL.Interface.Repositories;
using BP.DAL.Interface.Entities.Users;
using BP.DAL.Interface.Entities;
using BP.ORM;
using System.Data.Entity;
using Ninject;
using Ninject.Parameters;

namespace BP.DAL.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;
        private Dictionary<Type, object> repos;
        private IKernel kernel;
        private bool disposed;

        public UnitOfWork(DbContext context, IKernel kernel)
        {
            this.context = context;
            this.kernel = kernel;
            repos = new Dictionary<Type, object>();
        }

        public IRepository<T> GetRepository<T>() where T : class, IEntity
        {
            object repo;
            if (repos.TryGetValue(typeof(T), out repo))
                repos.Add(typeof(T), repo = kernel.Get<IRepository<T>>(
                    new Parameter("context", context, true)));

            return repo as IRepository<T>;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    context.Dispose();
            }
            disposed = true;
        }
    }
}
