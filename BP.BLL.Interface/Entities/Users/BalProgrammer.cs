using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.BLL.Interface.Entities.Users
{
    public class BalProgrammer : BalUser
    {
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public string About { get; set; }
        public IDictionary<BalSkill, int> Skills { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
