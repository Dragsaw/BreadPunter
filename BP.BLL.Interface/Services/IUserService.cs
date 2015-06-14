using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.BLL.Interface.Services
{
    public interface IUserService : IService<BalUser>
    {
        IEnumerable<BalUser> Get(IEnumerable<BalUserSkill> skills);
        void Create(string email, string password, BalRole role);
        bool Exists(string email, string password);
        string GetPasswordHash(string password);
    }
}
