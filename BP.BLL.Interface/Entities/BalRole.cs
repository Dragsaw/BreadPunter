﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.BLL.Interface.Entities
{
    public class BalRole : IBalEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
