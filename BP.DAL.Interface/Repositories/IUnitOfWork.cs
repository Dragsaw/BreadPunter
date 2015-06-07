using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BP.DAL.Interface.Entities;
using BP.DAL.Interface.Entities.Users;

namespace BP.DAL.Interface.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<DalUser> UserRepository { get; }
        IRepository<DalRole> RoleRepository { get; }
        IRepository<DalSkill> SkillRepository { get; }

        void Save();
    }
}
