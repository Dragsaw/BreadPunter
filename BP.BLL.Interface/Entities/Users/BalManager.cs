using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.BLL.Interface.Entities.Users
{
    public class BalManager : BalUser
    {
        public List<BalFilter> Filters { get; set; }
    }
}
