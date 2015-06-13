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
        IEnumerable<BalUser> Get(string name);
    }
}
