using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.BLL.Interface.Entities.Users
{
    public class BllManager : BllUser
    {
        public List<BllFilter> Filters { get; set; }
    }
}
