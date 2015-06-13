using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.BLL.Interface.Entities
{
    public class BalFilter : IBalEntity
    {
        public int Id { get; set; }
        public IDictionary<BalSkill, int> Skills { get; set; }
        public DateTime? LastViewed { get; set; }
    }
}
