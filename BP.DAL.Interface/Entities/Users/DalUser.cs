﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.DAL.Interface.Entities.Users
{
    public class DalUser : IEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DalRole Role { get; set; }

        public object[] GetId()
        {
            return new object[] { Id };
        }
    }
}
