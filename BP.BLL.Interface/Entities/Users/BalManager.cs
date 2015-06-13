using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.BLL.Interface.Entities.Users
{
    public class BalManager : BalUser
    {
        public IEnumerable<BalFilter> Filters { get; set; }
    }
}
