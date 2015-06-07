using BP.DAL.Concrete;
using BP.DAL.Interface.Entities;
using BP.DAL.Interface.Entities.Users;
using BP.DAL.Interface.Repositories;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.DependencyResolver
{
    public static class ResolverSettings
    {
        public static void Configure(this IKernel kernel)
        {
            kernel.Bind<IRepository<DalUser>>().To<Repository<DalUser>>();
            kernel.Bind<IRepository<DalSkill>>().To<Repository<DalSkill>>();
            kernel.Bind<IRepository<DalFilter>>().To<Repository<DalFilter>>();
            kernel.Bind<IKernel>().ToConstant(kernel);
        }
    }
}
