﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApi.Models
{
    public class ToDoItem
    {
            public long Id { get; set; }
        public string? Name { get; set; } = null;
        public bool IsComplete { get; set; }
    }
}
