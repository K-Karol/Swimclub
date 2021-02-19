using System;
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
        public DateTime TimeOfClass { get; set; }
        public Models.Student[] Students { get; set; }
        public Dictionary<int,bool> Attendance { get; set; }
    }
}
