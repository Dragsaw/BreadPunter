﻿using System;
using System.Collections.Generic;

namespace BP.DAL.Interface.Entities.Users
{
    public class DalProgrammer : DalUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public string About { get; set; }
        public IDictionary<DalSkill, int> Skills { get; set; }
        public DateTime BirthDate { get; set; }
    }
}