using BP.BLL.Interface.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.BLL.Interface.Entities
{
    public class BllUserSkill : IBllEntity
    {
        public int Id { get; set; }
        public BllProgrammer User { get; set; }
        public BllSkill Skill { get; set; }
        public int Level { get; set; }
    }
}
