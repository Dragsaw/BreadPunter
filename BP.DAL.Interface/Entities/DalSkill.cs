using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.DAL.Interface.Entities
{
    [Serializable]
    public class DalSkill : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public object[] GetId()
        {
            return new object[] { Id };
        }
    }
}
