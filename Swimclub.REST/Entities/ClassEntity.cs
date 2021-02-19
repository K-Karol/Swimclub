using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Swimclub.REST.Entities
{
    [Table("Classes")]
    public class Class
    {
        [Key, Required]
        public int ID { get; set; }
        [Required]
        public string Pool { get; set; }
        [Required]
        public int ClassGrade { get; set; }
        [Required]
        public int coachID { get; set; }
        [Required]
        public DateTime TimeOfClass { get; set; }
        [Required, NotMapped]
        public int[] StudentIDs { get; set; }
        public string StudentIDsJSON
		{
			get { return JsonConvert.SerializeObject(StudentIDs); }
            set { StudentIDs = JsonConvert.DeserializeObject<int[]>(value); }
        }
        [Required, NotMapped]
        public bool[] StudentAttendance { get; set; }
        public string StudentAttendanceJSON
        {
            get { return JsonConvert.SerializeObject(StudentAttendance); }
            set { StudentAttendance = JsonConvert.DeserializeObject<bool[]>(value); }
        }

    }
}
