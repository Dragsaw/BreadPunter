using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.DAL.Interface.Entities.Users
{
    public class DalManager : DalUser
    {
        public IEnumerable<DalFilter> Filters { get; set; }
    }
}
