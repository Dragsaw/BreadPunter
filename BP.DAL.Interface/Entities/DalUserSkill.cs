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
        public int Id
        {
            get
            {
                return int.Parse(string.Concat(User.Id.ToString(), Skill.Id.ToString()));
            }
        }
        public DalProgrammer User { get; set; }
        public DalSkill Skill { get; set; }
        public int Level { get; set; }
    }
}
