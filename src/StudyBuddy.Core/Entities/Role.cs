﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddy.Core.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<User> Users { get; set; } = new();
    }
}
