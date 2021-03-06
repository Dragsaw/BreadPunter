﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.DAL.Interface.Entities
{
    public class DalFilter : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public IDictionary<DalSkill, int> Skills { get; set; }
        public DateTime? LastViewed { get; set; }

        public object[] GetId()
        {
            return new object[] { Id };
        }
    }
}
