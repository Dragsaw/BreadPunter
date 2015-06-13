﻿using BP.DAL.Concrete;
using BP.DAL.Interface.Entities;
using BP.DAL.Interface.Entities.Users;
using BP.DAL.Interface.Repositories;
using Ninject;
using BP.ORM;
using BP.DAL.Concrete.Repositories;
using System.Data.Entity;
using BP.BLL.Concrete;
using BP.BLL.Interface.Services;
using BP.BLL.Interface.Entities.Users;
using Ninject.Web.Common;

namespace BP.DependencyResolver
{
    public static class ResolverSettings
    {
        public static void Configure(this IKernel kernel)
        {
            kernel.Bind<IRepository<DalUser>>().To<UserRepository>().InRequestScope();
            kernel.Bind<IRepository<DalSkill>>().To<SkillRepository>().InRequestScope();
            kernel.Bind<IRepository<DalUserSkill>>().To<UserSkillRepository>().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<DbContext>().To<DatabaseEntities>().InRequestScope();
            //kernel.Bind<IKernel>().ToConstant(kernel).InRequestScope();
            kernel.Bind<IService<BalUser>>().To<UserService>().InRequestScope();
        }
    }
}
