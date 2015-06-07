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

namespace BP.DAL.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;
        private IRepository<DalUser> userRepository;
        private IRepository<DalRole> roleRepository;
        private IRepository<DalSkill> skillRepository;
        private IRepository<DalFilter> filterRepository;
        private IKernel kernel;
        private bool disposed;

        public UnitOfWork(DbContext context, IKernel kernel)
        {
            this.context = context;
            this.kernel = kernel;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return GetExistingRepository(typeof(T)) as IRepository<T> ?? kernel.Get<IRepository<T>>();
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

        private object GetExistingRepository(Type type)
        {
            if (type == typeof(DalUser))
                return userRepository;
            else if (type == typeof(DalRole))
                return roleRepository;
            else if (type == typeof(DalSkill))
                return skillRepository;
            else return filterRepository;
        }
    }
}
