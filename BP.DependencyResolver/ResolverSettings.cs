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
using BP.ORM;
using BP.DAL.Concrete.Repositories;
using System.Data.Entity;

namespace BP.DependencyResolver
{
    public static class ResolverSettings
    {
        public static void Configure(this IKernel kernel)
        {
            kernel.Bind<IRepository<DalUser>>().To<UserRepository>();
            kernel.Bind<IRepository<DalSkill>>().To<SkillRepository>();
            kernel.Bind<DbContext>().To<DatabaseEntities>();
            kernel.Bind<IKernel>().ToConstant(kernel);
        }
    }
}
