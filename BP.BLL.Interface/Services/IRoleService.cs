using BP.BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.BLL.Interface.Services
{
    public interface IRoleService : IService<BllRole>
    {
        void Create(string name);
    }
}
