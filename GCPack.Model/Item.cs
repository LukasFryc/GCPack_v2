﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCPack.Model
{
    public class Item
    {
        public int ID { get; set; }
        public int OrderBy { get; set; }
        public string Value { get; set; }
        public string Code { get; set; } // Lukas 27.8.2017 na zvazeni
    }
}
