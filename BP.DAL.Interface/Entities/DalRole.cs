﻿using System.Collections.Generic;
using BP.DAL.Interface.Entities.Users;

namespace BP.DAL.Interface.Entities
{
    public class DalRole : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public object[] GetId()
        {
            return new object[] { Id };
        }
    }
}
