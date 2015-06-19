using BP.DAL.Concrete;
using BP.DAL.Interface.Entities;
using BP.DAL.Interface.Entities.Users;
using BP.DAL.Interface.Repositories;
using Ninject;
using BP.ORM;
using System.Data.Entity;
using BP.BLL.Concrete;
using BP.BLL.Interface.Services;
using BP.BLL.Interface.Entities.Users;
using Ninject.Web.Common;
using BP.BLL.Interface.Entities;
using BP.DAL.Interface.Mappers;
using BP.DAL.Mappers;

namespace BP.DependencyResolver
{
    public static class ResolverSettings
    {
        public static void Configure(this IKernel kernel)
        {
            kernel.Bind<DbContext>().To<DatabaseEntities>().InRequestScope();

            kernel.Bind<IRepository<DalUser>>().To<GenericRepository<User, DalUser>>().InRequestScope();
            kernel.Bind<IRepository<DalSkill>>().To<GenericRepository<Skill, DalSkill>>().InRequestScope();
            kernel.Bind<IRepository<DalRole>>().To<GenericRepository<Role, DalRole>>().InRequestScope();
            kernel.Bind<IRepository<DalUserSkill>>().To<GenericRepository<UserSkill, DalUserSkill>>().InRequestScope();
            kernel.Bind<IRepository<DalFilter>>().To<GenericRepository<Filter, DalFilter>>().InRequestScope();

            kernel.Bind<IMapper<Role, DalRole>>().To<RoleMapper>().InSingletonScope();
            kernel.Bind<IMapper<User, DalUser>>().To<UserMapper>().InSingletonScope();
            kernel.Bind<IMapper<Filter, DalFilter>>().To<FilterMapper>().InSingletonScope();
            kernel.Bind<IMapper<Skill, DalSkill>>().To<SkillMapper>().InSingletonScope();
            kernel.Bind<IMapper<UserSkill, DalUserSkill>>().To<UserSkillMapper>().InSingletonScope();

            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();

            kernel.Bind<IService<BllUser>>().To<UserService>().InRequestScope();
            kernel.Bind<IService<BllRole>>().To<RoleService>().InRequestScope();
            kernel.Bind<IService<BllSkill>>().To<SkillService>().InRequestScope();
            kernel.Bind<IRoleService>().To<RoleService>().InRequestScope();
            kernel.Bind<IUserService>().To<UserService>().InRequestScope();
        }
    }
}
