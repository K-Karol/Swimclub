﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub.Models
{
    public class Class
    {
        public int ID { get; set; }
        public string Pool { get; set; }
        public int ClassGrade { get; set; }
        public Models.User coach { get; set; }
        DateTime TimeOfClass { get; set; }
    }
}
