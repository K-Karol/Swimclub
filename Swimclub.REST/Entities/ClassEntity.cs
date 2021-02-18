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
        public int coachID { get; set; }
        [Required]
        DateTime TimeOfClass { get; set; }
    }
}
