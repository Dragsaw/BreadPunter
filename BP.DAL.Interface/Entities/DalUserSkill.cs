using BP.DAL.Interface.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.DAL.Interface.Entities
{
    public class DalUserSkill : IEntity
    {
        public DalProgrammer User { get; set; }
        public DalSkill Skill { get; set; }
        public int Level { get; set; }

        public object[] GetId()
        {
            return new object[] { User.Id, Skill.Id };
        }
    }
}
