using BP.BLL.Interface.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.BLL.Interface.Entities
{
    public class BalUserSkill : IBalEntity
    {
        public int Id { get; set; }
        public BalProgrammer User { get; set; }
        public BalSkill Skill { get; set; }
        public int Level { get; set; }
    }
}
