using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.BLL.Interface.Services
{
    public interface IUserService : IService<BllUser>
    {
        IEnumerable<BllUser> UsersInRole(int roleId);
        IEnumerable<BllUser> Get(IEnumerable<BllUserSkill> skills);
        void Create(string email, string password, BllRole role);
        bool Exists(string email, string password);
    }
}
